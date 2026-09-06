"use client";

import { useQuery } from "@tanstack/react-query";
import { useRouter } from "next/navigation";
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

export default function DashboardPage() {
  const router = useRouter();
  const user = getCurrentUser();

  useEffect(() => {
    if (!isAuthenticated()) {
      router.push("/login");
    }
  }, [router]);

  const { data: courses, isLoading, isError } = useQuery({
    queryKey: ["courses"],
    queryFn: () => api.get<Course[]>("/api/courses"),
    enabled: isAuthenticated(),
  });

  return (
    <main className="min-h-screen bg-gray-50">
      <header className="border-b border-gray-200 bg-white">
        <div className="mx-auto flex max-w-5xl items-center justify-between px-6 py-4">
          <div>
            <h1 className="text-xl font-bold text-gray-900">Enoneno&apos;s School</h1>
            <p className="text-sm text-gray-500">
              Bem-vindo, {user?.fullName || "Utilizador"}
            </p>
          </div>
          <div className="flex items-center gap-4">
            {user && (user.role === "Admin" || user.role === "Teacher") && (
              <Link
                href="/admin"
                className="rounded-lg border border-gray-300 px-3 py-1.5 text-sm text-gray-700 hover:bg-gray-100"
              >
                Administração
              </Link>
            )}
            <span className="rounded-full bg-gray-100 px-3 py-1 text-xs font-medium text-gray-600">
              {user?.role}
            </span>
            <button
              onClick={() => void logout()}
              className="rounded-lg border border-gray-300 px-3 py-1.5 text-sm text-gray-600 transition-colors hover:bg-gray-100"
            >
              Sair
            </button>
          </div>
        </div>
      </header>

      <div className="mx-auto max-w-5xl px-6 py-10">
        <h2 className="text-2xl font-bold text-gray-900">Cursos</h2>
        <p className="mt-1 text-gray-500">
          Selecione um curso para ver as aulas disponíveis
        </p>

        {isLoading ? (
          <div className="mt-8 text-center text-gray-500">A carregar...</div>
        ) : isError ? (
          <div className="mt-8 rounded-lg bg-red-50 p-6 text-center text-red-600">
            Erro ao carregar os cursos. Tente novamente.
          </div>
        ) : !courses || courses.length === 0 ? (
          <div className="mt-8 rounded-lg border border-dashed border-gray-300 p-12 text-center">
            <p className="text-gray-500">Nenhum curso disponível</p>
          </div>
        ) : (
          <div className="mt-8 grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
            {courses.map((course) => (
              <Link
                key={course.id}
                href={`/courses/${course.id}`}
                className="rounded-xl border border-gray-200 bg-white p-6 transition-shadow hover:shadow-md"
              >
                {course.imageUrl && (
                  // eslint-disable-next-line @next/next/no-img-element
                  <img
                    src={course.imageUrl}
                    alt={course.name}
                    className="mb-3 h-28 w-full rounded-lg object-cover"
                  />
                )}
                <h3 className="font-semibold text-gray-900">{course.name}</h3>
                <p className="mt-2 text-sm text-gray-500">
                  {course.description}
                </p>
              </Link>
            ))}
          </div>
        )}
      </div>
    </main>
  );
}
