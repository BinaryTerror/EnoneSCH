"use client";

import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import Link from "next/link";
import { api } from "@/lib/api";
import { isAuthenticated } from "@/lib/auth";

interface Course {
  id: string;
  name: string;
  description: string;
  imageUrl?: string | null;
}

export default function AdminCoursesPage() {
  const qc = useQueryClient();
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [message, setMessage] = useState("");

  const { data: courses, isLoading } = useQuery({
    queryKey: ["admin", "courses"],
    queryFn: () => api.get<Course[]>("/api/admin/courses"),
    enabled: isAuthenticated(),
  });

  const createMutation = useMutation({
    mutationFn: () =>
      api.post("/api/admin/courses", { name, description, imageUrl: null }),
    onSuccess: () => {
      setName("");
      setDescription("");
      setMessage("Curso criado com sucesso.");
      qc.invalidateQueries({ queryKey: ["admin", "courses"] });
      qc.invalidateQueries({ queryKey: ["courses"] });
    },
    onError: (e) =>
      setMessage("Erro: " + (e instanceof Error ? e.message : "tente novamente")),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => api.delete(`/api/admin/courses/${id}`),
    onSuccess: () => {
      setMessage("Curso eliminado.");
      qc.invalidateQueries({ queryKey: ["admin", "courses"] });
      qc.invalidateQueries({ queryKey: ["courses"] });
    },
    onError: (e) =>
      setMessage("Erro: " + (e instanceof Error ? e.message : "tente novamente")),
  });

  return (
    <div>
      <h2 className="text-2xl font-bold text-gray-900">Cursos</h2>

      <form
        onSubmit={(e) => {
          e.preventDefault();
          createMutation.mutate();
        }}
        className="mt-6 rounded-xl border border-gray-200 bg-white p-6"
      >
        <h3 className="font-semibold text-gray-900">Criar curso</h3>
        <div className="mt-4 grid gap-4">
          <input
            placeholder="Nome do curso"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
            className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm outline-none focus:ring-2 focus:ring-blue-500"
          />
          <textarea
            placeholder="Descrição"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            rows={2}
            className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
        <div className="mt-4 flex items-center justify-between">
          <button
            type="submit"
            disabled={createMutation.isPending}
            className="rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700 disabled:opacity-50"
          >
            {createMutation.isPending ? "A criar..." : "Criar curso"}
          </button>
          {message && <span className="text-sm text-gray-600">{message}</span>}
        </div>
      </form>

      <div className="mt-8">
        <h3 className="font-semibold text-gray-900">Cursos existentes</h3>
        {isLoading ? (
          <p className="mt-4 text-gray-500">A carregar...</p>
        ) : (
          <div className="mt-4 space-y-3">
            {(courses ?? []).map((course) => (
              <div
                key={course.id}
                className="flex items-center justify-between rounded-xl border border-gray-200 bg-white p-4"
              >
                <div>
                  <p className="font-semibold text-gray-900">{course.name}</p>
                  <p className="text-sm text-gray-500">{course.description}</p>
                </div>
                <div className="flex items-center gap-3">
                  <Link
                    href={`/admin/courses/${course.id}`}
                    className="rounded-lg bg-gray-100 px-3 py-1.5 text-sm text-gray-700 hover:bg-gray-200"
                  >
                    Gerir
                  </Link>
                  <button
                    onClick={() => deleteMutation.mutate(course.id)}
                    disabled={deleteMutation.isPending}
                    className="rounded-lg bg-red-50 px-3 py-1.5 text-sm text-red-600 hover:bg-red-100"
                  >
                    Eliminar
                  </button>
                </div>
              </div>
            ))}
            {!courses || courses.length === 0 ? (
              <p className="text-gray-500">Nenhum curso criado.</p>
            ) : null}
          </div>
        )}
      </div>
    </div>
  );
}