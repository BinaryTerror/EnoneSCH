"use client";

import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import Link from "next/link";
import { api } from "@/lib/api";
import { isAuthenticated } from "@/lib/auth";

interface AcademicYear {
  id: string;
  yearNumber: number;
}

export default function AdminCourseYearsPage() {
  const params = useParams();
  const courseId = params.id as string;
  const qc = useQueryClient();
  const [yearNumber, setYearNumber] = useState("");
  const [message, setMessage] = useState("");

  const { data: years, isLoading } = useQuery({
    queryKey: ["admin", "years", courseId],
    queryFn: () =>
      api.get<AcademicYear[]>(`/api/admin/academic-years?courseId=${courseId}`),
    enabled: isAuthenticated(),
  });

  const createMutation = useMutation({
    mutationFn: () =>
      api.post(`/api/admin/academic-years/${courseId}`, {
        yearNumber: Number(yearNumber),
      }),
    onSuccess: () => {
      setYearNumber("");
      setMessage("Ano criado.");
      qc.invalidateQueries({ queryKey: ["admin", "years", courseId] });
    },
    onError: (e) =>
      setMessage("Erro: " + (e instanceof Error ? e.message : "tente novamente")),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => api.delete(`/api/admin/academic-years/${id}`),
    onSuccess: () => {
      setMessage("Ano eliminado.");
      qc.invalidateQueries({ queryKey: ["admin", "years", courseId] });
    },
  });

  return (
    <div>
      <Link
        href="/admin/courses"
        className="text-sm text-gray-500 hover:text-gray-700"
      >
        ← Voltar aos cursos
      </Link>
      <h2 className="mt-2 text-2xl font-bold text-gray-900">Anos letivos do curso</h2>

      <form
        onSubmit={(e) => {
          e.preventDefault();
          createMutation.mutate();
        }}
        className="mt-6 flex items-end gap-3 rounded-xl border border-gray-200 bg-white p-6"
      >
        <div>
          <label className="text-sm font-medium text-gray-700">Nº do ano</label>
          <input
            type="number"
            min={1}
            value={yearNumber}
            onChange={(e) => setYearNumber(e.target.value)}
            required
            className="mt-1 w-32 rounded-lg border border-gray-300 px-3 py-2 text-sm outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
        <button
          type="submit"
          disabled={createMutation.isPending}
          className="rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700 disabled:opacity-50"
        >
          Criar ano
        </button>
        {message && <span className="text-sm text-gray-600">{message}</span>}
      </form>

      <div className="mt-8 space-y-3">
        {isLoading ? (
          <p className="text-gray-500">A carregar...</p>
        ) : (
          (years ?? []).map((year) => (
            <div
              key={year.id}
              className="flex items-center justify-between rounded-xl border border-gray-200 bg-white p-4"
            >
              <p className="font-semibold text-gray-900">{year.yearNumber}º Ano</p>
              <div className="flex items-center gap-3">
                <Link
                  href={`/admin/courses/${courseId}/years/${year.id}`}
                  className="rounded-lg bg-gray-100 px-3 py-1.5 text-sm text-gray-700 hover:bg-gray-200"
                >
                  Semestres
                </Link>
                <button
                  onClick={() => deleteMutation.mutate(year.id)}
                  className="rounded-lg bg-red-50 px-3 py-1.5 text-sm text-red-600 hover:bg-red-100"
                >
                  Eliminar
                </button>
              </div>
            </div>
          ))
        )}
        {years && years.length === 0 ? (
          <p className="text-gray-500">Nenhum ano criado.</p>
        ) : null}
      </div>
    </div>
  );
}