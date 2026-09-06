"use client";

import { useEffect } from "react";
import { useQuery } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import Link from "next/link";
import { api } from "@/lib/api";
import { isAuthenticated } from "@/lib/auth";

interface LessonProgress {
  id: string;
  studentId: string;
  lessonId: string;
  watchedSeconds: number;
  completed: boolean;
  updatedAt: string;
}

export default function LessonProgressPage() {
  const params = useParams();
  const lessonId = params.lessonId as string;

  useEffect(() => {
    if (!isAuthenticated()) {
      window.location.href = "/login";
    }
  }, []);

  const progressQuery = useQuery({
    queryKey: ["progress", "me", lessonId],
    queryFn: async () => {
      try {
        return await api.get<LessonProgress>(`/api/progress/lesson/${lessonId}`);
      } catch {
        return null;
      }
    },
    enabled: isAuthenticated(),
  });

  const progress = progressQuery.data;

  return (
    <main className="min-h-screen bg-gray-50">
      <div className="mx-auto max-w-3xl px-6 py-10">
        <Link href="/dashboard" className="text-sm text-gray-500 hover:text-gray-700">
          ← Voltar
        </Link>
        <h1 className="mt-2 text-2xl font-bold text-gray-900">Progresso da aula</h1>

        {progressQuery.isLoading ? (
          <p className="mt-6 text-gray-500">A carregar...</p>
        ) : !progress ? (
          <div className="mt-6 rounded-xl border border-dashed border-gray-300 bg-white p-10 text-center text-gray-500">
            Ainda não começaste esta aula.
          </div>
        ) : (
          <div className="mt-6 rounded-xl border border-gray-200 bg-white p-6">
            <div className="flex items-center gap-4">
              <div
                className={`flex h-12 w-12 items-center justify-center rounded-full text-xl ${
                  progress.completed ? "bg-green-100" : "bg-blue-100"
                }`}
              >
                {progress.completed ? "✅" : "▶️"}
              </div>
              <div>
                <p className="font-semibold text-gray-900">
                  {progress.completed ? "Aula concluída" : "Em curso"}
                </p>
                <p className="text-sm text-gray-500">
                  {Math.floor(progress.watchedSeconds / 60)}m{" "}
                  {progress.watchedSeconds % 60}s assistidos
                </p>
              </div>
            </div>
            <div className="mt-4 h-2 w-full overflow-hidden rounded-full bg-gray-100">
              <div className="h-full rounded-full bg-blue-500" />
            </div>
            <p className="mt-3 text-xs text-gray-400">
              Atualizado: {new Date(progress.updatedAt).toLocaleString()}
            </p>
          </div>
        )}
      </div>
    </main>
  );
}