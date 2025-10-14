-- Script untuk memperbaiki IconUrl Invoice App
-- Mengganti URL Wikipedia dengan ikon Material Design yang tepat

UPDATE apps 
SET icon_url = 'mdi-file-document-edit-outline' 
WHERE code = 'invoice-app' AND name = 'Invoice App';

-- Untuk memastikan perubahan berhasil, cek hasil
SELECT id, name, code, icon_url FROM apps WHERE code = 'invoice-app';