"use client";

import { useRef, useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import Link from "next/link";
import { api } from "@/lib/api";
import { isAuthenticated } from "@/lib/auth";

interface LessonVideo {
  id: string;
  status: string;
  durationSeconds: number;
  streamUrl: string;
}

export default function AdminLessonVideoPage() {
  const params = useParams();
  const lessonId = params.lessonId as string;
  const qc = useQueryClient();
  const fileInputRef = useRef<HTMLInputElement>(null);
  const [message, setMessage] = useState("");

  const videoQuery = useQuery({
    queryKey: ["admin", "video", lessonId],
    queryFn: async () =>
      api.get<LessonVideo>(`/api/lessons/${lessonId}/video`).catch(() => null),
    enabled: isAuthenticated(),
  });

  const uploadMutation = useMutation({
    mutationFn: (file: File) => {
      const formData = new FormData();
      formData.append("file", file);
      return api.upload(`/api/admin/videos/${lessonId}`, formData);
    },
    onSuccess: () => {
      setMessage("Vídeo enviado com sucesso.");
      if (fileInputRef.current) fileInputRef.current.value = "";
      qc.invalidateQueries({ queryKey: ["admin", "video", lessonId] });
    },
    onError: (e) =>
      setMessage("Erro no upload: " + (e instanceof Error ? e.message : "")),
  });

  const deleteMutation = useMutation({
    mutationFn: (videoId: string) => api.delete(`/api/admin/videos/${videoId}`),
    onSuccess: () => {
      setMessage("Vídeo removido.");
      qc.invalidateQueries({ queryKey: ["admin", "video", lessonId] });
    },
  });

  const video = videoQuery.data;

  return (
    <div>
      <Link href="/admin/courses" className="text-sm text-gray-500 hover:text-gray-700">
        ← Voltar
      </Link>
      <h2 className="mt-2 text-2xl font-bold text-gray-900">Vídeo da aula</h2>

      <div className="mt-6 rounded-xl border border-gray-200 bg-white p-6">
        {video ? (
          <div className="mb-4 rounded-lg bg-green-50 p-4 text-sm text-green-700">
            Vídeo presente — estado: <strong>{video.status}</strong>
            {deleteMutation.isPending ? null : (
              <button
                onClick={() => deleteMutation.mutate(video.id)}
                className="ml-4 rounded-lg bg-red-50 px-3 py-1 text-sm text-red-600 hover:bg-red-100"
              >
                Remover vídeo
              </button>
            )}
          </div>
        ) : (
          <p className="text-sm text-gray-500">
            Ainda não existe vídeo para esta aula.
          </p>
        )}

        <form
          onSubmit={(e) => {
            e.preventDefault();
            const file = fileInputRef.current?.files?.[0];
            if (file) uploadMutation.mutate(file);
            else setMessage("Seleciona um ficheiro de vídeo.");
          }}
          className="mt-4"
        >
          <input
            ref={fileInputRef}
            type="file"
            accept="video/*"
            className="block w-full text-sm text-gray-600 file:mr-4 file:rounded-lg file:border-0 file:bg-blue-600 file:px-4 file:py-2 file:text-sm file:text-white"
          />
          <button
            type="submit"
            disabled={uploadMutation.isPending}
            className="mt-4 rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700 disabled:opacity-50"
          >
            {uploadMutation.isPending ? "A enviar..." : "Enviar vídeo"}
          </button>
          {message && <span className="ml-4 text-sm text-gray-600">{message}</span>}
        </form>
      </div>

      {video && (
        <div className="mt-6 overflow-hidden rounded-xl border border-gray-200 bg-black">
          <video
            controls
            src={`${process.env.NEXT_PUBLIC_API_URL || "http://localhost:5282"}${
              video.streamUrl
            }`}
            className="aspect-video w-full"
          />
        </div>
      )}
    </div>
  );
}