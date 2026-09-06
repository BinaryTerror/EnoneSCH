"use client";

import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import Link from "next/link";
import { api } from "@/lib/api";
import { isAuthenticated } from "@/lib/auth";

interface Semester {
  id: string;
  semesterNumber: number;
}

export default function AdminYearSemestersPage() {
  const params = useParams();
  const courseId = params.id as string;
  const yearId = params.yearId as string;
  const qc = useQueryClient();
  const [semesterNumber, setSemesterNumber] = useState("");
  const [message, setMessage] = useState("");

  const { data: semesters, isLoading } = useQuery({
    queryKey: ["admin", "semesters", yearId],
    queryFn: () =>
      api.get<Semester[]>(`/api/admin/semesters?academicYearId=${yearId}`),
    enabled: isAuthenticated(),
  });

  const createMutation = useMutation({
    mutationFn: () =>
      api.post(`/api/admin/semesters/${yearId}`, {
        semesterNumber: Number(semesterNumber),
      }),
    onSuccess: () => {
      setSemesterNumber("");
      setMessage("Semestre criado.");
      qc.invalidateQueries({ queryKey: ["admin", "semesters", yearId] });
    },
    onError: (e) =>
      setMessage("Erro: " + (e instanceof Error ? e.message : "tente novamente")),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => api.delete(`/api/admin/semesters/${id}`),
    onSuccess: () => {
      setMessage("Semestre eliminado.");
      qc.invalidateQueries({ queryKey: ["admin", "semesters", yearId] });
    },
  });

  return (
    <div>
      <Link
        href={`/admin/courses/${courseId}`}
        className="text-sm text-gray-500 hover:text-gray-700"
      >
        ← Voltar aos anos
      </Link>
      <h2 className="mt-2 text-2xl font-bold text-gray-900">Semestres do ano</h2>

      <form
        onSubmit={(e) => {
          e.preventDefault();
          createMutation.mutate();
        }}
        className="mt-6 flex items-end gap-3 rounded-xl border border-gray-200 bg-white p-6"
      >
        <div>
          <label className="text-sm font-medium text-gray-700">Nº do semestre</label>
          <input
            type="number"
            min={1}
            value={semesterNumber}
            onChange={(e) => setSemesterNumber(e.target.value)}
            required
            className="mt-1 w-32 rounded-lg border border-gray-300 px-3 py-2 text-sm outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
        <button
          type="submit"
          disabled={createMutation.isPending}
          className="rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700 disabled:opacity-50"
        >
          Criar semestre
        </button>
        {message && <span className="text-sm text-gray-600">{message}</span>}
      </form>

      <div className="mt-8 space-y-3">
        {isLoading ? (
          <p className="text-gray-500">A carregar...</p>
        ) : (
          (semesters ?? []).map((semester) => (
            <div
              key={semester.id}
              className="flex items-center justify-between rounded-xl border border-gray-200 bg-white p-4"
            >
              <p className="font-semibold text-gray-900">
                {semester.semesterNumber}º Semestre
              </p>
              <div className="flex items-center gap-3">
                <Link
                  href={`/admin/courses/${courseId}/years/${yearId}/semesters/${semester.id}`}
                  className="rounded-lg bg-gray-100 px-3 py-1.5 text-sm text-gray-700 hover:bg-gray-200"
                >
                  Disciplinas
                </Link>
                <button
                  onClick={() => deleteMutation.mutate(semester.id)}
                  className="rounded-lg bg-red-50 px-3 py-1.5 text-sm text-red-600 hover:bg-red-100"
                >
                  Eliminar
                </button>
              </div>
            </div>
          ))
        )}
        {semesters && semesters.length === 0 ? (
          <p className="text-gray-500">Nenhum semestre criado.</p>
        ) : null}
      </div>
    </div>
  );
}