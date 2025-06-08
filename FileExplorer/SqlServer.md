QUERY PARA CREAR BASE DE DATOS DE UNIVERSIDADES EN SQL SERVER


-- BORRAR TABLAS SI EXISTEN
DROP TABLE IF EXISTS Contenidos;
DROP TABLE IF EXISTS Materias;
DROP TABLE IF EXISTS Semestres;
DROP TABLE IF EXISTS Carreras;
DROP TABLE IF EXISTS Facultades;
DROP TABLE IF EXISTS Campus;
DROP TABLE IF EXISTS Universidades;

-- CREAR TABLAS

CREATE TABLE Universidades (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL
);

CREATE TABLE Campus (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    UniversidadId INT NOT NULL,
    FOREIGN KEY (UniversidadId) REFERENCES Universidades(Id)
);

CREATE TABLE Facultades (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    CampusId INT NOT NULL,
    FOREIGN KEY (CampusId) REFERENCES Campus(Id)
);

CREATE TABLE Carreras (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    FacultadId INT NOT NULL,
    FOREIGN KEY (FacultadId) REFERENCES Facultades(Id)
);

CREATE TABLE Semestres (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(50) NOT NULL,
    CarreraId INT NOT NULL,
    FOREIGN KEY (CarreraId) REFERENCES Carreras(Id)
);


CREATE TABLE Materias (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    SemestreId INT NOT NULL,
    FOREIGN KEY (SemestreId) REFERENCES Semestres(Id)
);

CREATE TABLE Contenidos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MateriaId INT NOT NULL,
    Numero NVARCHAR(10) NOT NULL, -- ejemplo: '1.1', '2.4' etc . . .
    Titulo NVARCHAR(255) NOT NULL, -- Por ejemplo: 'Historia de los patrones de software'
    FOREIGN KEY (MateriaId) REFERENCES Materias(Id)
);

CREATE TABLE TiposArchivo (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Extension NVARCHAR(10),
    MimeType NVARCHAR(100)
);

CREATE TABLE RecursosContenido (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ContenidoId INT NOT NULL,
    Nombre NVARCHAR(200) NOT NULL,
    Url NVARCHAR(500) NOT NULL,
    TipoArchivoId INT NOT NULL,
    Descripcion NVARCHAR(500),
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ContenidoId) REFERENCES Contenidos(Id),
    FOREIGN KEY (TipoArchivoId) REFERENCES TiposArchivo(Id)
);


-- INSERTAR DATOS BASE

-- Universidad
INSERT INTO Universidades (Nombre) VALUES ('UABC');

-- Campus
INSERT INTO Campus (Nombre, UniversidadId) VALUES ('Ensenada', 1);

-- Facultades
INSERT INTO Facultades (Nombre, CampusId)
VALUES 
('Facultad de Artes', 1),
('Facultad de Ciencias', 1),
('Facultad de Ciencias Marinas', 1),
('Facultad de Deportes', 1),
('Facultad de Ingenieria', 1);

-- Carreras - Facultad de Artes
INSERT INTO Carreras (Nombre, FacultadId)
VALUES 
('Artes Visuales', 1),
('Artes Musicales', 1),
('Artes Teatrales', 1),
('Artes Literarias', 1);

-- Carreras - Facultad de Ciencias
INSERT INTO Carreras (Nombre, FacultadId) VALUES
('Biologia', 2),
('Matematicas', 2),
('Fisica', 2),
('Quimica', 2);

-- Carreras - Facultad de Ciencias Marinas
INSERT INTO Carreras (Nombre, FacultadId) VALUES
('Biologia Marina', 3),
('Oceanologia', 3),
('Ciencias del Mar', 3),
('Ingenieria en Ciencias del Mar', 3),
('Ingenieria en Transporte Maritimo', 3);

-- Carreras - Facultad de Deportes
INSERT INTO Carreras (Nombre, FacultadId) VALUES
('Ciencias del Deporte', 4),
('Entrenamiento Deportivo', 4),
('Rehabilitacion y Terapia Fisica', 4),
('Nutricion y Dietetica', 4),
('Educacion Fisica', 4);

-- Carreras - Facultad de Ingenieria
INSERT INTO Carreras (Nombre, FacultadId) VALUES
('Tronco Comun de Ingenieria', 5),
('Ingenieria Civil', 5),
('Ingenieria en Electronica', 5),
('Ingenieria en Computacion', 5),
('Ingenieria Industrial', 5),
('Bioingenieria', 5),
('Software y Tecnologias Emergentes', 5);

-- Agregar 8 semestres a cada carrera
INSERT INTO Semestres (Nombre, CarreraId)
SELECT 
    CONCAT('Semestre ', s.Numero) AS Nombre,
    c.Id AS CarreraId
FROM Carreras c
CROSS JOIN (
    SELECT 1 AS Numero UNION ALL
    SELECT 2 UNION ALL
    SELECT 3 UNION ALL
    SELECT 4 UNION ALL
    SELECT 5 UNION ALL
    SELECT 6 UNION ALL
    SELECT 7 UNION ALL
    SELECT 8
) s;



-- MATERIAS PARA "Tronco Comun de Ingenieria" (solo Semestre 1 y 2)

INSERT INTO Materias (Nombre, SemestreId)
SELECT M.Nombre, S.Id
FROM (
    -- Materias por semestre
    SELECT 'Matemáticas Básicas' AS Nombre, 1 AS NumSem
    UNION ALL SELECT 'Física General', 1
    UNION ALL SELECT 'Fundamentos de Programación', 1
    UNION ALL SELECT 'Introducción a la Ingeniería', 1
    UNION ALL SELECT 'Redacción Técnica', 1

    UNION ALL SELECT 'Álgebra Lineal', 2
    UNION ALL SELECT 'Cálculo Diferencial', 2
    UNION ALL SELECT 'Dibujo Técnico', 2
    UNION ALL SELECT 'Estadística Básica', 2
    UNION ALL SELECT 'Química Aplicada', 2
) M
JOIN Semestres S ON S.Nombre = CONCAT('Semestre ', M.NumSem)
JOIN Carreras C ON C.Id = S.CarreraId AND C.Nombre = 'Tronco Comun de Ingenieria';

-- MATERIAS PARA "Ingenieria Civil" (Semestres 3 al 8)
INSERT INTO Materias (Nombre, SemestreId)
SELECT M.Nombre, S.Id
FROM (
    -- Semestre 3
    SELECT 'Estática' AS Nombre, 3 AS NumSem
    UNION ALL SELECT 'Topografía', 3
    UNION ALL SELECT 'Mecánica de Materiales', 3
    UNION ALL SELECT 'Geología Aplicada', 3
    UNION ALL SELECT 'Cálculo Integral', 3

    -- Semestre 4
    UNION ALL SELECT 'Dinámica', 4
    UNION ALL SELECT 'Hidráulica Básica', 4
    UNION ALL SELECT 'Estructuras I', 4
    UNION ALL SELECT 'Geotecnia', 4
    UNION ALL SELECT 'Construcción I', 4

    -- Semestre 5
    UNION ALL SELECT 'Diseño de Concreto', 5
    UNION ALL SELECT 'Mecánica de Suelos', 5
    UNION ALL SELECT 'Hidrología', 5
    UNION ALL SELECT 'Costos y Presupuestos', 5
    UNION ALL SELECT 'Estructuras II', 5

    -- Semestre 6
    UNION ALL SELECT 'Diseño de Pavimentos', 6
    UNION ALL SELECT 'Planeación Urbana', 6
    UNION ALL SELECT 'Tratamiento de Aguas', 6
    UNION ALL SELECT 'Construcción II', 6
    UNION ALL SELECT 'Evaluación de Proyectos', 6

    -- Semestre 7
    UNION ALL SELECT 'Estructuras Metálicas', 7
    UNION ALL SELECT 'Diseño de Puentes', 7
    UNION ALL SELECT 'Gestión Ambiental', 7
    UNION ALL SELECT 'Proyectos de Ingeniería', 7
    UNION ALL SELECT 'Túneles y Obras Subterráneas', 7

    -- Semestre 8
    UNION ALL SELECT 'Supervisión de Obras', 8
    UNION ALL SELECT 'Ingeniería Sísmica', 8
    UNION ALL SELECT 'Ética Profesional', 8
    UNION ALL SELECT 'Prácticas Profesionales', 8
    UNION ALL SELECT 'Seminario de Titulación', 8
) M
JOIN Semestres S ON S.Nombre = CONCAT('Semestre ', M.NumSem)
JOIN Carreras C ON C.Id = S.CarreraId AND C.Nombre = 'Ingenieria Civil';


-- MATERIAS PARA "Ingenieria en Electronica" (Semestres 3 al 8)
INSERT INTO Materias (Nombre, SemestreId)
SELECT M.Nombre, S.Id
FROM (
    -- Semestre 3
    SELECT 'Circuitos Eléctricos I' AS Nombre, 3 AS NumSem
    UNION ALL SELECT 'Electromagnetismo', 3
    UNION ALL SELECT 'Dispositivos Electrónicos', 3
    UNION ALL SELECT 'Matemáticas Avanzadas', 3
    UNION ALL SELECT 'Programación Aplicada', 3

    -- Semestre 4
    UNION ALL SELECT 'Circuitos Eléctricos II', 4
    UNION ALL SELECT 'Análisis de Señales', 4
    UNION ALL SELECT 'Electrónica Digital I', 4
    UNION ALL SELECT 'Instrumentación Básica', 4
    UNION ALL SELECT 'Estadística Aplicada', 4

    -- Semestre 5
    UNION ALL SELECT 'Electrónica Analógica', 5
    UNION ALL SELECT 'Microcontroladores I', 5
    UNION ALL SELECT 'Diseño Lógico', 5
    UNION ALL SELECT 'Comunicaciones I', 5
    UNION ALL SELECT 'Control I', 5

    -- Semestre 6
    UNION ALL SELECT 'Electrónica de Potencia', 6
    UNION ALL SELECT 'Microcontroladores II', 6
    UNION ALL SELECT 'Procesamiento de Señales', 6
    UNION ALL SELECT 'Redes de Comunicaciones', 6
    UNION ALL SELECT 'Control II', 6

    -- Semestre 7
    UNION ALL SELECT 'Diseño de Sistemas Embebidos', 7
    UNION ALL SELECT 'Sistemas Digitales Avanzados', 7
    UNION ALL SELECT 'Optoelectrónica', 7
    UNION ALL SELECT 'Proyecto Electrónico I', 7
    UNION ALL SELECT 'Administración de Proyectos', 7

    -- Semestre 8
    UNION ALL SELECT 'Automatización Industrial', 8
    UNION ALL SELECT 'Sistemas de Control Avanzado', 8
    UNION ALL SELECT 'Proyecto Electrónico II', 8
    UNION ALL SELECT 'Ética y Sociedad', 8
    UNION ALL SELECT 'Prácticas Profesionales', 8
) M
JOIN Semestres S ON S.Nombre = CONCAT('Semestre ', M.NumSem)
JOIN Carreras C ON C.Id = S.CarreraId AND C.Nombre = 'Ingenieria en Electronica';


-- MATERIAS PARA "Ingenieria en Computacion" (Semestres 3 al 8)
INSERT INTO Materias (Nombre, SemestreId)
SELECT M.Nombre, S.Id
FROM (
    -- Semestre 3
    SELECT 'Estructuras de Datos' AS Nombre, 3 AS NumSem
    UNION ALL SELECT 'Arquitectura de Computadoras I', 3
    UNION ALL SELECT 'Matemáticas Discretas', 3
    UNION ALL SELECT 'Bases de Datos I', 3
    UNION ALL SELECT 'Programación Orientada a Objetos', 3

    -- Semestre 4
    UNION ALL SELECT 'Sistemas Operativos', 4
    UNION ALL SELECT 'Redes de Computadoras I', 4
    UNION ALL SELECT 'Teoría de la Computación', 4
    UNION ALL SELECT 'Bases de Datos II', 4
    UNION ALL SELECT 'Desarrollo Web', 4

    -- Semestre 5
    UNION ALL SELECT 'Ingeniería de Software I', 5
    UNION ALL SELECT 'Arquitectura de Computadoras II', 5
    UNION ALL SELECT 'Redes de Computadoras II', 5
    UNION ALL SELECT 'Desarrollo Móvil', 5
    UNION ALL SELECT 'Algoritmos Avanzados', 5

    -- Semestre 6
    UNION ALL SELECT 'Ingeniería de Software II', 6
    UNION ALL SELECT 'Seguridad Informática', 6
    UNION ALL SELECT 'Inteligencia Artificial', 6
    UNION ALL SELECT 'Computación en la Nube', 6
    UNION ALL SELECT 'Computación Gráfica', 6

    -- Semestre 7
    UNION ALL SELECT 'Compiladores', 7
    UNION ALL SELECT 'Machine Learning', 7
    UNION ALL SELECT 'Desarrollo de Videojuegos', 7
    UNION ALL SELECT 'Proyecto de Software I', 7
    UNION ALL SELECT 'Auditoría de Sistemas', 7

    -- Semestre 8
    UNION ALL SELECT 'Proyecto de Software II', 8
    UNION ALL SELECT 'Ética Profesional y Sociedad', 8
    UNION ALL SELECT 'Administración de TI', 8
    UNION ALL SELECT 'Blockchain y Criptomonedas', 8
    UNION ALL SELECT 'Prácticas Profesionales', 8
) M
JOIN Semestres S ON S.Nombre = CONCAT('Semestre ', M.NumSem)
JOIN Carreras C ON C.Id = S.CarreraId AND C.Nombre = 'Ingenieria en Computacion';

-- MATERIAS PARA "Ingenieria Industrial" (Semestres 3 al 8)
INSERT INTO Materias (Nombre, SemestreId)
SELECT M.Nombre, S.Id
FROM (
    -- Semestre 3
    SELECT 'Estadística I' AS Nombre, 3 AS NumSem
    UNION ALL SELECT 'Procesos de Manufactura I', 3
    UNION ALL SELECT 'Dibujo Técnico Industrial', 3
    UNION ALL SELECT 'Contabilidad de Costos', 3
    UNION ALL SELECT 'Física Aplicada', 3

    -- Semestre 4
    UNION ALL SELECT 'Estadística II', 4
    UNION ALL SELECT 'Ingeniería de Métodos I', 4
    UNION ALL SELECT 'Ergonomía', 4
    UNION ALL SELECT 'Simulación de Procesos', 4
    UNION ALL SELECT 'Investigación de Operaciones I', 4

    -- Semestre 5
    UNION ALL SELECT 'Planeación y Control de la Producción', 5
    UNION ALL SELECT 'Ingeniería de Métodos II', 5
    UNION ALL SELECT 'Logística y Cadenas de Suministro', 5
    UNION ALL SELECT 'Calidad Total', 5
    UNION ALL SELECT 'Sistemas de Manufactura', 5

    -- Semestre 6
    UNION ALL SELECT 'Automatización Industrial', 6
    UNION ALL SELECT 'Finanzas Industriales', 6
    UNION ALL SELECT 'Gestión de Proyectos', 6
    UNION ALL SELECT 'Análisis de Decisiones', 6
    UNION ALL SELECT 'Investigación de Operaciones II', 6

    -- Semestre 7
    UNION ALL SELECT 'Formulación y Evaluación de Proyectos', 7
    UNION ALL SELECT 'Mantenimiento Industrial', 7
    UNION ALL SELECT 'Sistemas de Calidad', 7
    UNION ALL SELECT 'Gestión Ambiental', 7
    UNION ALL SELECT 'Simulación Avanzada', 7

    -- Semestre 8
    UNION ALL SELECT 'Prácticas Profesionales', 8
    UNION ALL SELECT 'Administración de la Producción', 8
    UNION ALL SELECT 'Ética Profesional y Sociedad', 8
    UNION ALL SELECT 'Ingeniería de Costos', 8
    UNION ALL SELECT 'Taller de Investigación', 8
) M
JOIN Semestres S ON S.Nombre = CONCAT('Semestre ', M.NumSem)
JOIN Carreras C ON C.Id = S.CarreraId AND C.Nombre = 'Ingenieria Industrial';


-- MATERIAS PARA "Bioingenieria" (Semestres 3 al 8)
INSERT INTO Materias (Nombre, SemestreId)
SELECT M.Nombre, S.Id
FROM (
    -- Semestre 3
    SELECT 'Biología Celular' AS Nombre, 3 AS NumSem
    UNION ALL SELECT 'Bioquímica', 3
    UNION ALL SELECT 'Fundamentos de Ingeniería', 3
    UNION ALL SELECT 'Fisiología Humana', 3
    UNION ALL SELECT 'Cálculo Vectorial', 3

    -- Semestre 4
    UNION ALL SELECT 'Microbiología Industrial', 4
    UNION ALL SELECT 'Procesos Biotecnológicos', 4
    UNION ALL SELECT 'Termodinámica', 4
    UNION ALL SELECT 'Bioinstrumentación I', 4
    UNION ALL SELECT 'Probabilidad y Estadística', 4

    -- Semestre 5
    UNION ALL SELECT 'Genética', 5
    UNION ALL SELECT 'Bioinformática', 5
    UNION ALL SELECT 'Transferencia de Masa y Calor', 5
    UNION ALL SELECT 'Bioprocesos', 5
    UNION ALL SELECT 'Ingeniería de Reactores', 5

    -- Semestre 6
    UNION ALL SELECT 'Bioinstrumentación II', 6
    UNION ALL SELECT 'Diseño de Experimentos', 6
    UNION ALL SELECT 'Biomateriales', 6
    UNION ALL SELECT 'Regulación Sanitaria', 6
    UNION ALL SELECT 'Fisiología de Sistemas', 6

    -- Semestre 7
    UNION ALL SELECT 'Sistemas de Control Biológico', 7
    UNION ALL SELECT 'Tópicos Avanzados en Bioingeniería', 7
    UNION ALL SELECT 'Gestión de Proyectos Biotecnológicos', 7
    UNION ALL SELECT 'Simulación de Procesos Biológicos', 7
    UNION ALL SELECT 'Innovación Tecnológica', 7

    -- Semestre 8
    UNION ALL SELECT 'Prácticas Profesionales', 8
    UNION ALL SELECT 'Diseño de Equipos Biomédicos', 8
    UNION ALL SELECT 'Emprendimiento en Bioingeniería', 8
    UNION ALL SELECT 'Bioética y Legislación', 8
    UNION ALL SELECT 'Seminario de Titulación', 8
) M
JOIN Semestres S ON S.Nombre = CONCAT('Semestre ', M.NumSem)
JOIN Carreras C ON C.Id = S.CarreraId AND C.Nombre = 'Bioingenieria';


-- MATERIAS PARA "Software y Tecnologias Emergentes" (Semestres 3 al 8)
INSERT INTO Materias (Nombre, SemestreId)
SELECT M.Nombre, S.Id
FROM (
    -- Semestre 3
    SELECT 'Estructuras de Datos' AS Nombre, 3 AS NumSem
    UNION ALL SELECT 'Matemáticas Discretas', 3
    UNION ALL SELECT 'Sistemas Operativos I', 3
    UNION ALL SELECT 'Fundamentos de Bases de Datos', 3
    UNION ALL SELECT 'Desarrollo Web I', 3

    -- Semestre 4
    UNION ALL SELECT 'Bases de Datos Avanzadas', 4
    UNION ALL SELECT 'Sistemas Operativos II', 4
    UNION ALL SELECT 'Redes de Computadoras', 4
    UNION ALL SELECT 'Desarrollo Web II', 4
    UNION ALL SELECT 'Ingeniería de Software I', 4

    -- Semestre 5
    UNION ALL SELECT 'Inteligencia Artificial', 5
    UNION ALL SELECT 'Ingeniería de Software II', 5
    UNION ALL SELECT 'Arquitectura de Software', 5
    UNION ALL SELECT 'Ciberseguridad', 5
    UNION ALL SELECT 'Desarrollo de Aplicaciones Móviles', 5

    -- Semestre 6
    UNION ALL SELECT 'Programación Concurrente', 6
    UNION ALL SELECT 'Cloud Computing', 6
    UNION ALL SELECT 'Blockchain y Criptomonedas', 6
    UNION ALL SELECT 'Interacción Humano-Computadora', 6
    UNION ALL SELECT 'Gestión de Proyectos de Software', 6

    -- Semestre 7
    UNION ALL SELECT 'Internet de las Cosas (IoT)', 7
    UNION ALL SELECT 'DevOps y Automatización', 7
    UNION ALL SELECT 'Big Data', 7
    UNION ALL SELECT 'Realidad Aumentada y Virtual', 7
    UNION ALL SELECT 'Tópicos Selectos en Software Emergente', 7

    -- Semestre 8
    UNION ALL SELECT 'Proyecto Integrador', 8
    UNION ALL SELECT 'Ética Profesional en TI', 8
    UNION ALL SELECT 'Innovación y Emprendimiento Tecnológico', 8
    UNION ALL SELECT 'Auditoría y Calidad de Software', 8
    UNION ALL SELECT 'Seminario de Titulación', 8
) M
JOIN Semestres S ON S.Nombre = CONCAT('Semestre ', M.NumSem)
JOIN Carreras C ON C.Id = S.CarreraId AND C.Nombre = 'Software y Tecnologias Emergentes';


-- CONTENIDOS DE EJEMPLO PARA LA MATERIA "Arquitectura de Software"
INSERT INTO Contenidos (MateriaId, Numero, Titulo)
SELECT M.Id, C.Numero, C.Titulo
FROM Materias M
JOIN Semestres S ON S.Id = M.SemestreId
JOIN Carreras Cr ON Cr.Id = S.CarreraId
CROSS JOIN (
    SELECT '1.1' AS Numero, 'Historia de los patrones de software' AS Titulo
    UNION ALL SELECT '1.2', 'Definición y estructura de los patrones de software'
    UNION ALL SELECT '1.3', 'Niveles en los patrones de software'
    UNION ALL SELECT '2.1', 'Definición de patrón arquitectónico'
    UNION ALL SELECT '2.2', 'Patrones estructurales'
    UNION ALL SELECT '2.3', 'Patrones para sistemas distribuidos'
    UNION ALL SELECT '2.4', 'Patrones para sistemas interactivos'
    UNION ALL SELECT '2.5', 'Patrones para sistemas adaptables'
) C
WHERE M.Nombre = 'Arquitectura de Software' AND Cr.Nombre = 'Software y Tecnologias Emergentes';


INSERT INTO TiposArchivo (Nombre, Extension, MimeType) VALUES
('Documento PDF', '.pdf', 'application/pdf'),
('Presentación PowerPoint', '.pptx', 'application/vnd.openxmlformats-officedocument.presentationml.presentation'),
('Documento Word', '.docx', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'),
('Video MP4', '.mp4', 'video/mp4'),
('Imagen JPEG', '.jpg', 'image/jpeg');


-- Eliminar semestres 1 y 2 de todas las carreras de Ingeniería excepto Tronco Común
DELETE FROM Semestres
WHERE Nombre IN ('Semestre 1', 'Semestre 2')
  AND CarreraId IN (
    SELECT Id FROM Carreras
    WHERE FacultadId = 5 -- Facultad de Ingeniería
      AND Nombre <> 'Tronco Comun de Ingenieria'
);

-- Eliminar semestres 3 a 8 de la carrera Tronco Comun de Ingenieria
DELETE FROM Semestres
WHERE Nombre IN ('Semestre 3', 'Semestre 4', 'Semestre 5', 'Semestre 6', 'Semestre 7', 'Semestre 8')
  AND CarreraId = (
    SELECT Id FROM Carreras
    WHERE Nombre = 'Tronco Comun de Ingenieria'
      AND FacultadId = 5
);
