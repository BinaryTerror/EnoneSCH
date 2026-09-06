"use client";

import { useQuery } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import Link from "next/link";
import { useEffect } from "react";
import { api } from "@/lib/api";
import { logout, isAuthenticated } from "@/lib/auth";

interface Semester {
  id: string;
  semesterNumber: number;
}

interface AcademicYear {
  id: string;
  yearNumber: number;
}

export default function YearDetailPage() {
  const params = useParams();
  const courseId = params.id as string;
  const yearId = params.yearId as string;

  useEffect(() => {
    if (!isAuthenticated()) {
      window.location.href = "/login";
    }
  }, []);

  const yearQuery = useQuery({
    queryKey: ["academicYear", yearId],
    queryFn: () => api.get<AcademicYear>(`/api/academic-years/${yearId}`),
    enabled: isAuthenticated(),
  });

  const semestersQuery = useQuery({
    queryKey: ["semesters", yearId],
    queryFn: () => api.get<Semester[]>(`/api/academic-years/${yearId}/semesters`),
    enabled: isAuthenticated(),
  });

  if (yearQuery.isLoading || semestersQuery.isLoading) {
    return (
      <main className="flex min-h-screen items-center justify-center bg-gray-50">
        <p className="text-gray-500">A carregar...</p>
      </main>
    );
  }

  if (yearQuery.isError || !yearQuery.data) {
    return (
      <main className="flex min-h-screen items-center justify-center bg-gray-50">
        <div className="text-center">
          <p className="text-gray-500">Ano não encontrado</p>
          <Link href={`/courses/${courseId}`} className="mt-4 inline-block text-blue-600 underline">
            Voltar
          </Link>
        </div>
      </main>
    );
  }

  const year = yearQuery.data;
  const semesters = semestersQuery.data ?? [];

  return (
    <main className="min-h-screen bg-gray-50">
      <header className="border-b border-gray-200 bg-white">
        <div className="mx-auto flex max-w-5xl items-center justify-between px-6 py-4">
          <div>
            <Link href={`/courses/${courseId}`} className="text-sm text-gray-500 hover:text-gray-700">
              ← Voltar ao curso
            </Link>
            <h1 className="text-xl font-bold text-gray-900">{year.yearNumber}º Ano</h1>
          </div>
          <button
            onClick={() => void logout()}
            className="rounded-lg border border-gray-300 px-3 py-1.5 text-sm text-gray-600 hover:bg-gray-100"
          >
            Sair
          </button>
        </div>
      </header>

      <div className="mx-auto max-w-5xl px-6 py-10">
        <h2 className="text-2xl font-bold text-gray-900">Semestres</h2>
        <p className="mt-1 text-gray-500">Seleciona um semestre para ver as disciplinas</p>

        {semesters.length === 0 ? (
          <div className="mt-8 rounded-lg border border-dashed border-gray-300 p-12 text-center">
            <p className="text-gray-500">Nenhum semestre disponível</p>
          </div>
        ) : (
          <div className="mt-8 grid gap-4 sm:grid-cols-2">
            {semesters.map((semester) => (
              <Link
                key={semester.id}
                href={`/courses/${courseId}/years/${yearId}/semesters/${semester.id}`}
                className="rounded-xl border border-gray-200 bg-white p-8 text-center transition-shadow hover:shadow-md"
              >
                <div className="text-4xl font-bold text-green-600">{semester.semesterNumber}º</div>
                <div className="mt-2 text-sm text-gray-500">Semestre</div>
              </Link>
            ))}
          </div>
        )}
      </div>
    </main>
  );
}