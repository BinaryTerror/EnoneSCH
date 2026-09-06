"use client";

import { useQuery } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import Link from "next/link";
import { useEffect } from "react";
import { api } from "@/lib/api";
import { logout, isAuthenticated } from "@/lib/auth";

interface Subject {
  id: string;
  name: string;
  description: string;
}

interface Lesson {
  id: string;
  title: string;
  description: string;
  position: number;
}

export default function SubjectDetailPage() {
  const params = useParams();
  const courseId = params.id as string;
  const yearId = params.yearId as string;
  const semesterId = params.semesterId as string;
  const subjectId = params.subjectId as string;

  useEffect(() => {
    if (!isAuthenticated()) {
      window.location.href = "/login";
    }
  }, []);

  const subjectQuery = useQuery({
    queryKey: ["subject", subjectId],
    queryFn: () => api.get<Subject>(`/api/subjects/${subjectId}`),
    enabled: isAuthenticated(),
  });

  const lessonsQuery = useQuery({
    queryKey: ["lessons", subjectId],
    queryFn: () => api.get<Lesson[]>(`/api/subjects/${subjectId}/lessons`),
    enabled: isAuthenticated(),
  });

  if (lessonsQuery.isLoading) {
    return (
      <main className="flex min-h-screen items-center justify-center bg-gray-50">
        <p className="text-gray-500">A carregar...</p>
      </main>
    );
  }

  const subject = subjectQuery.data;
  const lessons = lessonsQuery.data ?? [];

  const base = `/courses/${courseId}/years/${yearId}/semesters/${semesterId}/subjects/${subjectId}`;

  return (
    <main className="min-h-screen bg-gray-50">
      <header className="border-b border-gray-200 bg-white">
        <div className="mx-auto flex max-w-5xl items-center justify-between px-6 py-4">
          <div>
            <Link
              href={`/courses/${courseId}/years/${yearId}/semesters/${semesterId}`}
              className="text-sm text-gray-500 hover:text-gray-700"
            >
              ← Voltar ao semestre
            </Link>
            <h1 className="text-xl font-bold text-gray-900">
              {subject?.name ?? "Disciplina"}
            </h1>
            {subject?.description && (
              <p className="text-sm text-gray-500">{subject.description}</p>
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

      <div className="mx-auto max-w-3xl px-6 py-10">
        <h2 className="text-2xl font-bold text-gray-900">Aulas</h2>
        <p className="mt-1 text-gray-500">Seleciona uma aula para assistir ao vídeo</p>

        {lessonsQuery.isError ? (
          <div className="mt-8 rounded-lg bg-red-50 p-6 text-center text-red-600">
            Erro ao carregar as aulas.
          </div>
        ) : lessons.length === 0 ? (
          <div className="mt-8 rounded-lg border border-dashed border-gray-300 p-12 text-center">
            <p className="text-gray-500">Nenhuma aula disponível</p>
          </div>
        ) : (
          <div className="mt-8 space-y-3">
            {lessons.map((lesson) => (
              <Link
                key={lesson.id}
                href={`${base}/lessons/${lesson.id}`}
                className="flex items-start gap-4 rounded-xl border border-gray-200 bg-white p-5 transition-shadow hover:shadow-md"
              >
                <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg bg-blue-600 text-sm font-bold text-white">
                  {lesson.position}
                </div>
                <div>
                  <h3 className="font-semibold text-gray-900">{lesson.title}</h3>
                  <p className="mt-1 text-sm text-gray-500">{lesson.description}</p>
                </div>
              </Link>
            ))}
          </div>
        )}
      </div>
    </main>
  );
}