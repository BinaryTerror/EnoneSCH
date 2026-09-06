"use client";

import { useEffect, useRef } from "react";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import Link from "next/link";
import { api, API_BASE_URL } from "@/lib/api";
import { logout, isAuthenticated } from "@/lib/auth";

interface Lesson {
  id: string;
  title: string;
  description: string;
  position: number;
}

interface LessonVideo {
  id: string;
  status: string;
  durationSeconds: number;
  thumbnailUrl?: string | null;
  streamUrl: string;
}

interface LessonProgress {
  id: string;
  studentId: string;
  lessonId: string;
  watchedSeconds: number;
  completed: boolean;
}

export default function LessonDetailPage() {
  const params = useParams();
  const lessonId = params.lessonId as string;
  const queryClient = useQueryClient();

  const lastSavedAtRef = useRef(0);
  const lastSavedSecondsRef = useRef(0);

  useEffect(() => {
    if (!isAuthenticated()) {
      window.location.href = "/login";
    }
  }, []);

  const lessonQuery = useQuery({
    queryKey: ["lesson", lessonId],
    queryFn: () => api.get<Lesson>(`/api/lessons/${lessonId}`),
    enabled: isAuthenticated(),
  });

  const videoQuery = useQuery({
    queryKey: ["lessonVideo", lessonId],
    queryFn: () => api.get<LessonVideo>(`/api/lessons/${lessonId}/video`),
    enabled: isAuthenticated(),
    retry: false,
  });

  const progressQuery = useQuery({
    queryKey: ["progress", lessonId],
    queryFn: async () => {
      try {
        return await api.get<LessonProgress>(`/api/progress/lesson/${lessonId}`);
      } catch {
        return null;
      }
    },
    enabled: isAuthenticated(),
  });

  async function saveProgress(watchedSeconds: number, completed: boolean) {
    try {
      await api.post("/api/progress", { lessonId, watchedSeconds, completed });
      await queryClient.invalidateQueries({ queryKey: ["progress", lessonId] });
    } catch {
      // Falhas de progresso não devem interromper a reprodução.
    }
  }

  function handleTimeUpdate(e: React.SyntheticEvent<HTMLVideoElement>) {
    const video = e.currentTarget;
    const seconds = Math.floor(video.currentTime);
    const now = Date.now();

    const changedEnough = Math.abs(seconds - lastSavedSecondsRef.current) >= 5;
    const timeElapsed = now - lastSavedAtRef.current > 5000;

    if (!(changedEnough && (timeElapsed || seconds >= 5)) || seconds <= 0) return;

    lastSavedAtRef.current = now;
    lastSavedSecondsRef.current = seconds;
    void saveProgress(seconds, false);
  }

  function handleEnded(e: React.SyntheticEvent<HTMLVideoElement>) {
    const video = e.currentTarget;
    void saveProgress(Math.floor(video.duration || 0), true);
  }

  if (lessonQuery.isLoading) {
    return (
      <main className="flex min-h-screen items-center justify-center bg-gray-50">
        <p className="text-gray-500">A carregar...</p>
      </main>
    );
  }

  if (lessonQuery.isError || !lessonQuery.data) {
    return (
      <main className="flex min-h-screen items-center justify-center bg-gray-50">
        <div className="text-center">
          <p className="text-gray-500">Aula não encontrada</p>
          <Link
            href={`/courses/${params.id}`}
            className="mt-4 inline-block text-blue-600 underline"
          >
            Voltar
          </Link>
        </div>
      </main>
    );
  }

  const lesson = lessonQuery.data;
  const video = videoQuery.data;
  const progress = progressQuery.data;

  const backHref = `/courses/${params.id}/years/${params.yearId}/semesters/${params.semesterId}/subjects/${params.subjectId}`;
  const videoSrc = video ? `${API_BASE_URL}${video.streamUrl}` : null;
return (
    <main className="min-h-screen bg-gray-50">
      <header className="border-b border-gray-200 bg-white">
        <div className="mx-auto flex max-w-4xl items-center justify-between px-6 py-4">
          <div>
            <Link href={backHref} className="text-sm text-gray-500 hover:text-gray-700">
              ← Voltar às aulas
            </Link>
            <h1 className="text-xl font-bold text-gray-900">{lesson.title}</h1>
            {lesson.description && (
              <p className="text-sm text-gray-500">{lesson.description}</p>
            )}
          </div>
          <button
            onClick={() => void logout()}
            className="rounded-lg border border-gray-300 px-3 py-1.5 text-sm text-gray-600 hover:bg-gray-100"
          >
            Sair
          </button>
        </div>
      </header>

      <div className="mx-auto max-w-4xl px-6 py-10">
        {videoQuery.isLoading ? (
          <div className="rounded-xl border border-gray-200 bg-white p-12 text-center text-gray-500">
            A carregar vídeo...
          </div>
        ) : videoQuery.isError || !video ? (
          <div className="rounded-xl border border-dashed border-gray-300 bg-white p-12 text-center">
            <div className="text-5xl">🎬</div>
            <p className="mt-4 text-gray-500">
              Ainda não existe um vídeo para esta aula.
            </p>
          </div>
        ) : video.status !== "Ready" ? (
          <div className="rounded-xl border border-gray-200 bg-white p-12 text-center text-gray-500">
            O vídeo está a ser processado. Tenta novamente mais tarde.
          </div>
        ) : (
          <div className="overflow-hidden rounded-xl border border-gray-200 bg-black shadow-lg">
            <video
              controls
              autoPlay
              src={videoSrc ?? undefined}
              poster={video.thumbnailUrl ?? undefined}
              className="aspect-video w-full"
              onTimeUpdate={handleTimeUpdate}
              onEnded={handleEnded}
              onLoadedMetadata={(e) => {
                if (progress && progress.watchedSeconds > 0 && !progress.completed) {
                  const target = Math.min(
                    progress.watchedSeconds,
                    (e.currentTarget.duration || 0) - 1
                  );
                  if (target > 0) {
                    e.currentTarget.currentTime = target;
                  }
                }
              }}
            />
          </div>
        )}

        {progress && progress.completed && (
          <div className="mt-6 rounded-lg bg-green-50 p-4 text-sm text-green-700">
            ✅ Aula concluída — boa continuação!
          </div>
        )}
      </div>
    </main>
  );
}