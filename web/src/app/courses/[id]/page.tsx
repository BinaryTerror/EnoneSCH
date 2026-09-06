"use client";

import { useQuery } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import Link from "next/link";
import { useEffect } from "react";
import { api } from "@/lib/api";
import { getCurrentUser, logout, isAuthenticated } from "@/lib/auth";

interface Course {
  id: string;
  name: string;
  description: string;
  imageUrl?: string | null;
}

interface AcademicYear {
  id: string;
  yearNumber: number;
}

export default function CourseDetailPage() {
  const params = useParams();
  const courseId = params.id as string;
  const user = getCurrentUser();

  useEffect(() => {
    if (!isAuthenticated()) {
      window.location.href = "/login";
    }
  }, []);

  const courseQuery = useQuery({
    queryKey: ["course", courseId],
    queryFn: () => api.get<Course>(`/api/courses/${courseId}`),
    enabled: isAuthenticated(),
  });

  const yearsQuery = useQuery({
    queryKey: ["years", courseId],
    queryFn: () => api.get<AcademicYear[]>(`/api/courses/${courseId}/years`),
    enabled: isAuthenticated(),
  });

  if (courseQuery.isLoading || yearsQuery.isLoading) {
    return (
      <main className="flex min-h-screen items-center justify-center bg-gray-50">
        <p className="text-gray-500">A carregar...</p>
      </main>
    );
  }

  if (courseQuery.isError || !courseQuery.data) {
    return (
      <main className="flex min-h-screen items-center justify-center bg-gray-50">
        <div className="text-center">
          <p className="text-gray-500">Curso não encontrado</p>
          <Link href="/dashboard" className="mt-4 inline-block text-blue-600 underline">
            Voltar
          </Link>
        </div>
      </main>
    );
  }

  const course = courseQuery.data;
  const years = yearsQuery.data ?? [];

  return (
    <main className="min-h-screen bg-gray-50">
      <header className="border-b border-gray-200 bg-white">
        <div className="mx-auto flex max-w-5xl items-center justify-between px-6 py-4">
          <div>
            <Link href="/dashboard" className="text-sm text-gray-500 hover:text-gray-700">
              ← Voltar
            </Link>
            <h1 className="text-xl font-bold text-gray-900">{course.name}</h1>
            <p className="text-sm text-gray-500">{course.description}</p>
          </div>
          <div className="flex items-center gap-4">
            <span className="rounded-full bg-gray-100 px-3 py-1 text-xs font-medium text-gray-600">
              {user?.role}
            </span>
            <button
              onClick={() => void logout()}
              className="rounded-lg border border-gray-300 px-3 py-1.5 text-sm text-gray-600 hover:bg-gray-100"
            >
              Sair
            </button>
          </div>
        </div>
      </header>

      <div className="mx-auto max-w-5xl px-6 py-10">
        <h2 className="text-2xl font-bold text-gray-900">Anos Letivos</h2>
        <p className="mt-1 text-gray-500">Seleciona um ano para ver os semestres</p>

        {years.length === 0 ? (
          <div className="mt-8 rounded-lg border border-dashed border-gray-300 p-12 text-center">
            <p className="text-gray-500">Nenhum ano letivo disponível</p>
          </div>
        ) : (
          <div className="mt-8 grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
            {years.map((year) => (
              <Link
                key={year.id}
                href={`/courses/${courseId}/years/${year.id}`}
                className="rounded-xl border border-gray-200 bg-white p-6 text-center transition-shadow hover:shadow-md"
              >
                <div className="text-4xl font-bold text-blue-600">{year.yearNumber}º</div>
                <div className="mt-2 text-sm text-gray-500">Ano</div>
              </Link>
            ))}
          </div>
        )}
      </div>
    </main>
  );
}