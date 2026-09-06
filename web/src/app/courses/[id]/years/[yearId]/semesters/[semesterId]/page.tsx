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

interface Subject {
  id: string;
  name: string;
  description: string;
}

export default function SemesterDetailPage() {
  const params = useParams();
  const courseId = params.id as string;
  const yearId = params.yearId as string;
  const semesterId = params.semesterId as string;

  useEffect(() => {
    if (!isAuthenticated()) {
      window.location.href = "/login";
    }
  }, []);

  const semesterQuery = useQuery({
    queryKey: ["semester", semesterId],
    queryFn: () => api.get<Semester>(`/api/semesters/${semesterId}`),
    enabled: isAuthenticated(),
  });

  const subjectsQuery = useQuery({
    queryKey: ["subjects", semesterId],
    queryFn: () => api.get<Subject[]>(`/api/semesters/${semesterId}/subjects`),
    enabled: isAuthenticated(),
  });

  if (subjectsQuery.isLoading) {
    return (
      <main className="flex min-h-screen items-center justify-center bg-gray-50">
        <p className="text-gray-500">A carregar...</p>
      </main>
    );
  }

  const semester = semesterQuery.data;
  const subjects = subjectsQuery.data ?? [];

  return (
    <main className="min-h-screen bg-gray-50">
      <header className="border-b border-gray-200 bg-white">
        <div className="mx-auto flex max-w-5xl items-center justify-between px-6 py-4">
          <div>
            <Link
              href={`/courses/${courseId}/years/${yearId}`}
              className="text-sm text-gray-500 hover:text-gray-700"
            >
              ← Voltar ao ano
            </Link>
            <h1 className="text-xl font-bold text-gray-900">
              {semester ? `${semester.semesterNumber}º Semestre` : "Semestre"}
            </h1>
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
        <h2 className="text-2xl font-bold text-gray-900">Disciplinas</h2>
        <p className="mt-1 text-gray-500">Seleciona uma disciplina para ver as aulas</p>

        {subjectsQuery.isError ? (
          <div className="mt-8 rounded-lg bg-red-50 p-6 text-center text-red-600">
            Erro ao carregar as disciplinas.
          </div>
        ) : subjects.length === 0 ? (
          <div className="mt-8 rounded-lg border border-dashed border-gray-300 p-12 text-center">
            <p className="text-gray-500">Nenhuma disciplina disponível</p>
          </div>
        ) : (
          <div className="mt-8 grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
            {subjects.map((subject) => (
              <Link
                key={subject.id}
                href={`/courses/${courseId}/years/${yearId}/semesters/${semesterId}/subjects/${subject.id}`}
                className="rounded-xl border border-gray-200 bg-white p-6 transition-shadow hover:shadow-md"
              >
                <h3 className="font-semibold text-gray-900">{subject.name}</h3>
                <p className="mt-2 text-sm text-gray-500">{subject.description}</p>
              </Link>
            ))}
          </div>
        )}
      </div>
    </main>
  );
}