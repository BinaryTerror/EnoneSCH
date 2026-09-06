"use client";

import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import Link from "next/link";
import { api } from "@/lib/api";
import { isAuthenticated } from "@/lib/auth";

interface Subject {
  id: string;
  name: string;
  description: string;
}

export default function AdminSemesterSubjectsPage() {
  const params = useParams();
  const courseId = params.id as string;
  const yearId = params.yearId as string;
  const semesterId = params.semesterId as string;
  const qc = useQueryClient();
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [message, setMessage] = useState("");

  const { data: subjects, isLoading } = useQuery({
    queryKey: ["admin", "subjects", semesterId],
    queryFn: () => api.get<Subject[]>(`/api/admin/subjects?semesterId=${semesterId}`),
    enabled: isAuthenticated(),
  });

  const createMutation = useMutation({
    mutationFn: () =>
      api.post(`/api/admin/subjects/${semesterId}`, {
        name,
        description,
        imageUrl: null,
      }),
    onSuccess: () => {
      setName("");
      setDescription("");
      setMessage("Disciplina criada.");
      qc.invalidateQueries({ queryKey: ["admin", "subjects", semesterId] });
    },
    onError: (e) =>
      setMessage("Erro: " + (e instanceof Error ? e.message : "tente novamente")),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => api.delete(`/api/admin/subjects/${id}`),
    onSuccess: () => {
      setMessage("Disciplina eliminada.");
      qc.invalidateQueries({ queryKey: ["admin", "subjects", semesterId] });
    },
  });

  const backHref = `/admin/courses/${courseId}/years/${yearId}`;

  return (
    <div>
      <Link href={backHref} className="text-sm text-gray-500 hover:text-gray-700">
        ← Voltar aos semestres
      </Link>
      <h2 className="mt-2 text-2xl font-bold text-gray-900">Disciplinas do semestre</h2>

      <form
        onSubmit={(e) => {
          e.preventDefault();
          createMutation.mutate();
        }}
        className="mt-6 rounded-xl border border-gray-200 bg-white p-6"
      >
        <h3 className="font-semibold text-gray-900">Criar disciplina</h3>
        <div className="mt-4 grid gap-4">
          <input
            placeholder="Nome da disciplina"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
            className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm outline-none focus:ring-2 focus:ring-blue-500"
          />
          <input
            placeholder="Descrição"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            className="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
        <div className="mt-4 flex items-center justify-between">
          <button
            type="submit"
            disabled={createMutation.isPending}
            className="rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700 disabled:opacity-50"
          >
            Criar disciplina
          </button>
          {message && <span className="text-sm text-gray-600">{message}</span>}
        </div>
      </form>

      <div className="mt-8 space-y-3">
        {isLoading ? (
          <p className="text-gray-500">A carregar...</p>
        ) : (
          (subjects ?? []).map((subject) => (
            <div
              key={subject.id}
              className="flex items-center justify-between rounded-xl border border-gray-200 bg-white p-4"
            >
              <div>
                <p className="font-semibold text-gray-900">{subject.name}</p>
                <p className="text-sm text-gray-500">{subject.description}</p>
              </div>
              <div className="flex items-center gap-3">
                <Link
                  href={`/admin/subjects/${subject.id}`}
                  className="rounded-lg bg-gray-100 px-3 py-1.5 text-sm text-gray-700 hover:bg-gray-200"
                >
                  Aulas
                </Link>
                <button
                  onClick={() => deleteMutation.mutate(subject.id)}
                  className="rounded-lg bg-red-50 px-3 py-1.5 text-sm text-red-600 hover:bg-red-100"
                >
                  Eliminar
                </button>
              </div>
            </div>
          ))
        )}
        {subjects && subjects.length === 0 ? (
          <p className="text-gray-500">Nenhuma disciplina criada.</p>
        ) : null}
      </div>
    </div>
  );
}