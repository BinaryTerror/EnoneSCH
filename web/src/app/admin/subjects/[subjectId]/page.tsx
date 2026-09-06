"use client";

import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import Link from "next/link";
import { api } from "@/lib/api";
import { isAuthenticated } from "@/lib/auth";

interface Lesson {
  id: string;
  title: string;
  description: string;
  position: number;
}

export default function AdminSubjectLessonsPage() {
  const params = useParams();
  const subjectId = params.subjectId as string;
  const qc = useQueryClient();
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [position, setPosition] = useState("1");
  const [message, setMessage] = useState("");

  const { data: lessons, isLoading } = useQuery({
    queryKey: ["admin", "lessons", subjectId],
    queryFn: () => api.get<Lesson[]>(`/api/admin/lessons?subjectId=${subjectId}`),
    enabled: isAuthenticated(),
  });

  const createMutation = useMutation({
    mutationFn: () =>
      api.post(`/api/admin/lessons/${subjectId}`, {
        title,
        description,
        position: Number(position),
      }),
    onSuccess: () => {
      setTitle("");
      setDescription("");
      setPosition(String(Number(position) + 1));
      setMessage("Aula criada.");
      qc.invalidateQueries({ queryKey: ["admin", "lessons", subjectId] });
    },
    onError: (e) =>
      setMessage("Erro: " + (e instanceof Error ? e.message : "tente novamente")),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => api.delete(`/api/admin/lessons/${id}`),
    onSuccess: () => {
      setMessage("Aula eliminada.");
      qc.invalidateQueries({ queryKey: ["admin", "lessons", subjectId] });
    },
  });

  return (
    <div>
      <Link href="/admin/courses" className="text-sm text-gray-500 hover:text-gray-700">
        ← Voltar
      </Link>
      <h2 className="mt-2 text-2xl font-bold text-gray-900">Aulas da disciplina</h2>

      <form
        onSubmit={(e) => {
          e.preventDefault();
          createMutation.mutate();
        }}
        className="mt-6 rounded-xl border border-gray-200 bg-white p-6"
      >
        <h3 className="font-semibold text-gray-900">Criar aula</h3>
        <div className="mt-4 grid gap-4">
          <input
            placeholder="Título da aula"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
            className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm outline-none focus:ring-2 focus:ring-blue-500"
          />
          <input
            placeholder="Descrição"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm outline-none focus:ring-2 focus:ring-blue-500"
          />
          <input
            type="number"
            min={1}
            placeholder="Posição"
            value={position}
            onChange={(e) => setPosition(e.target.value)}
            className="w-40 rounded-lg border border-gray-300 px-3 py-2 text-sm outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
        <div className="mt-4 flex items-center justify-between">
          <button
            type="submit"
            disabled={createMutation.isPending}
            className="rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700 disabled:opacity-50"
          >
            Criar aula
          </button>
          {message && <span className="text-sm text-gray-600">{message}</span>}
        </div>
      </form>

      <div className="mt-8 space-y-3">
        {isLoading ? (
          <p className="text-gray-500">A carregar...</p>
        ) : (
          (lessons ?? []).map((lesson) => (
            <div
              key={lesson.id}
              className="flex items-center justify-between rounded-xl border border-gray-200 bg-white p-4"
            >
              <div className="flex items-start gap-3">
                <span className="flex h-8 w-8 items-center justify-center rounded-lg bg-blue-600 text-sm font-bold text-white">
                  {lesson.position}
                </span>
                <div>
                  <p className="font-semibold text-gray-900">{lesson.title}</p>
                  <p className="text-sm text-gray-500">{lesson.description}</p>
                </div>
              </div>
              <div className="flex items-center gap-3">
                <Link
                  href={`/admin/lessons/${lesson.id}`}
                  className="rounded-lg bg-gray-100 px-3 py-1.5 text-sm text-gray-700 hover:bg-gray-200"
                >
                  Vídeo
                </Link>
                <button
                  onClick={() => deleteMutation.mutate(lesson.id)}
                  className="rounded-lg bg-red-50 px-3 py-1.5 text-sm text-red-600 hover:bg-red-100"
                >
                  Eliminar
                </button>
              </div>
            </div>
          ))
        )}
        {lessons && lessons.length === 0 ? (
          <p className="text-gray-500">Nenhuma aula criada.</p>
        ) : null}
      </div>
    </div>
  );
}