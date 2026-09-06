-- Seed lessons: 2 lessons per subject
INSERT INTO lessons ("Id", "SubjectId", "Title", "Description", "Position", "CreatedAt", "UpdatedAt")
SELECT gen_random_uuid(), s."Id", s."Name" || ' - Aula 1', 'Introducao a ' || s."Name", 1, NOW(), NOW()
FROM subjects s
ON CONFLICT DO NOTHING;

INSERT INTO lessons ("Id", "SubjectId", "Title", "Description", "Position", "CreatedAt", "UpdatedAt")
SELECT gen_random_uuid(), s."Id", s."Name" || ' - Aula 2', 'Continuacao de ' || s."Name", 2, NOW(), NOW()
FROM subjects s
ON CONFLICT DO NOTHING;