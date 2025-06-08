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
    SELECT 'Matem�ticas B�sicas' AS Nombre, 1 AS NumSem
    UNION ALL SELECT 'F�sica General', 1
    UNION ALL SELECT 'Fundamentos de Programaci�n', 1
    UNION ALL SELECT 'Introducci�n a la Ingenier�a', 1
    UNION ALL SELECT 'Redacci�n T�cnica', 1

    UNION ALL SELECT '�lgebra Lineal', 2
    UNION ALL SELECT 'C�lculo Diferencial', 2
    UNION ALL SELECT 'Dibujo T�cnico', 2
    UNION ALL SELECT 'Estad�stica B�sica', 2
    UNION ALL SELECT 'Qu�mica Aplicada', 2
) M
JOIN Semestres S ON S.Nombre = CONCAT('Semestre ', M.NumSem)
JOIN Carreras C ON C.Id = S.CarreraId AND C.Nombre = 'Tronco Comun de Ingenieria';

-- MATERIAS PARA "Ingenieria Civil" (Semestres 3 al 8)
INSERT INTO Materias (Nombre, SemestreId)
SELECT M.Nombre, S.Id
FROM (
    -- Semestre 3
    SELECT 'Est�tica' AS Nombre, 3 AS NumSem
    UNION ALL SELECT 'Topograf�a', 3
    UNION ALL SELECT 'Mec�nica de Materiales', 3
    UNION ALL SELECT 'Geolog�a Aplicada', 3
    UNION ALL SELECT 'C�lculo Integral', 3

    -- Semestre 4
    UNION ALL SELECT 'Din�mica', 4
    UNION ALL SELECT 'Hidr�ulica B�sica', 4
    UNION ALL SELECT 'Estructuras I', 4
    UNION ALL SELECT 'Geotecnia', 4
    UNION ALL SELECT 'Construcci�n I', 4

    -- Semestre 5
    UNION ALL SELECT 'Dise�o de Concreto', 5
    UNION ALL SELECT 'Mec�nica de Suelos', 5
    UNION ALL SELECT 'Hidrolog�a', 5
    UNION ALL SELECT 'Costos y Presupuestos', 5
    UNION ALL SELECT 'Estructuras II', 5

    -- Semestre 6
    UNION ALL SELECT 'Dise�o de Pavimentos', 6
    UNION ALL SELECT 'Planeaci�n Urbana', 6
    UNION ALL SELECT 'Tratamiento de Aguas', 6
    UNION ALL SELECT 'Construcci�n II', 6
    UNION ALL SELECT 'Evaluaci�n de Proyectos', 6

    -- Semestre 7
    UNION ALL SELECT 'Estructuras Met�licas', 7
    UNION ALL SELECT 'Dise�o de Puentes', 7
    UNION ALL SELECT 'Gesti�n Ambiental', 7
    UNION ALL SELECT 'Proyectos de Ingenier�a', 7
    UNION ALL SELECT 'T�neles y Obras Subterr�neas', 7

    -- Semestre 8
    UNION ALL SELECT 'Supervisi�n de Obras', 8
    UNION ALL SELECT 'Ingenier�a S�smica', 8
    UNION ALL SELECT '�tica Profesional', 8
    UNION ALL SELECT 'Pr�cticas Profesionales', 8
    UNION ALL SELECT 'Seminario de Titulaci�n', 8
) M
JOIN Semestres S ON S.Nombre = CONCAT('Semestre ', M.NumSem)
JOIN Carreras C ON C.Id = S.CarreraId AND C.Nombre = 'Ingenieria Civil';


-- MATERIAS PARA "Ingenieria en Electronica" (Semestres 3 al 8)
INSERT INTO Materias (Nombre, SemestreId)
SELECT M.Nombre, S.Id
FROM (
    -- Semestre 3
    SELECT 'Circuitos El�ctricos I' AS Nombre, 3 AS NumSem
    UNION ALL SELECT 'Electromagnetismo', 3
    UNION ALL SELECT 'Dispositivos Electr�nicos', 3
    UNION ALL SELECT 'Matem�ticas Avanzadas', 3
    UNION ALL SELECT 'Programaci�n Aplicada', 3

    -- Semestre 4
    UNION ALL SELECT 'Circuitos El�ctricos II', 4
    UNION ALL SELECT 'An�lisis de Se�ales', 4
    UNION ALL SELECT 'Electr�nica Digital I', 4
    UNION ALL SELECT 'Instrumentaci�n B�sica', 4
    UNION ALL SELECT 'Estad�stica Aplicada', 4

    -- Semestre 5
    UNION ALL SELECT 'Electr�nica Anal�gica', 5
    UNION ALL SELECT 'Microcontroladores I', 5
    UNION ALL SELECT 'Dise�o L�gico', 5
    UNION ALL SELECT 'Comunicaciones I', 5
    UNION ALL SELECT 'Control I', 5

    -- Semestre 6
    UNION ALL SELECT 'Electr�nica de Potencia', 6
    UNION ALL SELECT 'Microcontroladores II', 6
    UNION ALL SELECT 'Procesamiento de Se�ales', 6
    UNION ALL SELECT 'Redes de Comunicaciones', 6
    UNION ALL SELECT 'Control II', 6

    -- Semestre 7
    UNION ALL SELECT 'Dise�o de Sistemas Embebidos', 7
    UNION ALL SELECT 'Sistemas Digitales Avanzados', 7
    UNION ALL SELECT 'Optoelectr�nica', 7
    UNION ALL SELECT 'Proyecto Electr�nico I', 7
    UNION ALL SELECT 'Administraci�n de Proyectos', 7

    -- Semestre 8
    UNION ALL SELECT 'Automatizaci�n Industrial', 8
    UNION ALL SELECT 'Sistemas de Control Avanzado', 8
    UNION ALL SELECT 'Proyecto Electr�nico II', 8
    UNION ALL SELECT '�tica y Sociedad', 8
    UNION ALL SELECT 'Pr�cticas Profesionales', 8
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
    UNION ALL SELECT 'Matem�ticas Discretas', 3
    UNION ALL SELECT 'Bases de Datos I', 3
    UNION ALL SELECT 'Programaci�n Orientada a Objetos', 3

    -- Semestre 4
    UNION ALL SELECT 'Sistemas Operativos', 4
    UNION ALL SELECT 'Redes de Computadoras I', 4
    UNION ALL SELECT 'Teor�a de la Computaci�n', 4
    UNION ALL SELECT 'Bases de Datos II', 4
    UNION ALL SELECT 'Desarrollo Web', 4

    -- Semestre 5
    UNION ALL SELECT 'Ingenier�a de Software I', 5
    UNION ALL SELECT 'Arquitectura de Computadoras II', 5
    UNION ALL SELECT 'Redes de Computadoras II', 5
    UNION ALL SELECT 'Desarrollo M�vil', 5
    UNION ALL SELECT 'Algoritmos Avanzados', 5

    -- Semestre 6
    UNION ALL SELECT 'Ingenier�a de Software II', 6
    UNION ALL SELECT 'Seguridad Inform�tica', 6
    UNION ALL SELECT 'Inteligencia Artificial', 6
    UNION ALL SELECT 'Computaci�n en la Nube', 6
    UNION ALL SELECT 'Computaci�n Gr�fica', 6

    -- Semestre 7
    UNION ALL SELECT 'Compiladores', 7
    UNION ALL SELECT 'Machine Learning', 7
    UNION ALL SELECT 'Desarrollo de Videojuegos', 7
    UNION ALL SELECT 'Proyecto de Software I', 7
    UNION ALL SELECT 'Auditor�a de Sistemas', 7

    -- Semestre 8
    UNION ALL SELECT 'Proyecto de Software II', 8
    UNION ALL SELECT '�tica Profesional y Sociedad', 8
    UNION ALL SELECT 'Administraci�n de TI', 8
    UNION ALL SELECT 'Blockchain y Criptomonedas', 8
    UNION ALL SELECT 'Pr�cticas Profesionales', 8
) M
JOIN Semestres S ON S.Nombre = CONCAT('Semestre ', M.NumSem)
JOIN Carreras C ON C.Id = S.CarreraId AND C.Nombre = 'Ingenieria en Computacion';

-- MATERIAS PARA "Ingenieria Industrial" (Semestres 3 al 8)
INSERT INTO Materias (Nombre, SemestreId)
SELECT M.Nombre, S.Id
FROM (
    -- Semestre 3
    SELECT 'Estad�stica I' AS Nombre, 3 AS NumSem
    UNION ALL SELECT 'Procesos de Manufactura I', 3
    UNION ALL SELECT 'Dibujo T�cnico Industrial', 3
    UNION ALL SELECT 'Contabilidad de Costos', 3
    UNION ALL SELECT 'F�sica Aplicada', 3

    -- Semestre 4
    UNION ALL SELECT 'Estad�stica II', 4
    UNION ALL SELECT 'Ingenier�a de M�todos I', 4
    UNION ALL SELECT 'Ergonom�a', 4
    UNION ALL SELECT 'Simulaci�n de Procesos', 4
    UNION ALL SELECT 'Investigaci�n de Operaciones I', 4

    -- Semestre 5
    UNION ALL SELECT 'Planeaci�n y Control de la Producci�n', 5
    UNION ALL SELECT 'Ingenier�a de M�todos II', 5
    UNION ALL SELECT 'Log�stica y Cadenas de Suministro', 5
    UNION ALL SELECT 'Calidad Total', 5
    UNION ALL SELECT 'Sistemas de Manufactura', 5

    -- Semestre 6
    UNION ALL SELECT 'Automatizaci�n Industrial', 6
    UNION ALL SELECT 'Finanzas Industriales', 6
    UNION ALL SELECT 'Gesti�n de Proyectos', 6
    UNION ALL SELECT 'An�lisis de Decisiones', 6
    UNION ALL SELECT 'Investigaci�n de Operaciones II', 6

    -- Semestre 7
    UNION ALL SELECT 'Formulaci�n y Evaluaci�n de Proyectos', 7
    UNION ALL SELECT 'Mantenimiento Industrial', 7
    UNION ALL SELECT 'Sistemas de Calidad', 7
    UNION ALL SELECT 'Gesti�n Ambiental', 7
    UNION ALL SELECT 'Simulaci�n Avanzada', 7

    -- Semestre 8
    UNION ALL SELECT 'Pr�cticas Profesionales', 8
    UNION ALL SELECT 'Administraci�n de la Producci�n', 8
    UNION ALL SELECT '�tica Profesional y Sociedad', 8
    UNION ALL SELECT 'Ingenier�a de Costos', 8
    UNION ALL SELECT 'Taller de Investigaci�n', 8
) M
JOIN Semestres S ON S.Nombre = CONCAT('Semestre ', M.NumSem)
JOIN Carreras C ON C.Id = S.CarreraId AND C.Nombre = 'Ingenieria Industrial';


-- MATERIAS PARA "Bioingenieria" (Semestres 3 al 8)
INSERT INTO Materias (Nombre, SemestreId)
SELECT M.Nombre, S.Id
FROM (
    -- Semestre 3
    SELECT 'Biolog�a Celular' AS Nombre, 3 AS NumSem
    UNION ALL SELECT 'Bioqu�mica', 3
    UNION ALL SELECT 'Fundamentos de Ingenier�a', 3
    UNION ALL SELECT 'Fisiolog�a Humana', 3
    UNION ALL SELECT 'C�lculo Vectorial', 3

    -- Semestre 4
    UNION ALL SELECT 'Microbiolog�a Industrial', 4
    UNION ALL SELECT 'Procesos Biotecnol�gicos', 4
    UNION ALL SELECT 'Termodin�mica', 4
    UNION ALL SELECT 'Bioinstrumentaci�n I', 4
    UNION ALL SELECT 'Probabilidad y Estad�stica', 4

    -- Semestre 5
    UNION ALL SELECT 'Gen�tica', 5
    UNION ALL SELECT 'Bioinform�tica', 5
    UNION ALL SELECT 'Transferencia de Masa y Calor', 5
    UNION ALL SELECT 'Bioprocesos', 5
    UNION ALL SELECT 'Ingenier�a de Reactores', 5

    -- Semestre 6
    UNION ALL SELECT 'Bioinstrumentaci�n II', 6
    UNION ALL SELECT 'Dise�o de Experimentos', 6
    UNION ALL SELECT 'Biomateriales', 6
    UNION ALL SELECT 'Regulaci�n Sanitaria', 6
    UNION ALL SELECT 'Fisiolog�a de Sistemas', 6

    -- Semestre 7
    UNION ALL SELECT 'Sistemas de Control Biol�gico', 7
    UNION ALL SELECT 'T�picos Avanzados en Bioingenier�a', 7
    UNION ALL SELECT 'Gesti�n de Proyectos Biotecnol�gicos', 7
    UNION ALL SELECT 'Simulaci�n de Procesos Biol�gicos', 7
    UNION ALL SELECT 'Innovaci�n Tecnol�gica', 7

    -- Semestre 8
    UNION ALL SELECT 'Pr�cticas Profesionales', 8
    UNION ALL SELECT 'Dise�o de Equipos Biom�dicos', 8
    UNION ALL SELECT 'Emprendimiento en Bioingenier�a', 8
    UNION ALL SELECT 'Bio�tica y Legislaci�n', 8
    UNION ALL SELECT 'Seminario de Titulaci�n', 8
) M
JOIN Semestres S ON S.Nombre = CONCAT('Semestre ', M.NumSem)
JOIN Carreras C ON C.Id = S.CarreraId AND C.Nombre = 'Bioingenieria';


-- MATERIAS PARA "Software y Tecnologias Emergentes" (Semestres 3 al 8)
INSERT INTO Materias (Nombre, SemestreId)
SELECT M.Nombre, S.Id
FROM (
    -- Semestre 3
    SELECT 'Estructuras de Datos' AS Nombre, 3 AS NumSem
    UNION ALL SELECT 'Matem�ticas Discretas', 3
    UNION ALL SELECT 'Sistemas Operativos I', 3
    UNION ALL SELECT 'Fundamentos de Bases de Datos', 3
    UNION ALL SELECT 'Desarrollo Web I', 3

    -- Semestre 4
    UNION ALL SELECT 'Bases de Datos Avanzadas', 4
    UNION ALL SELECT 'Sistemas Operativos II', 4
    UNION ALL SELECT 'Redes de Computadoras', 4
    UNION ALL SELECT 'Desarrollo Web II', 4
    UNION ALL SELECT 'Ingenier�a de Software I', 4

    -- Semestre 5
    UNION ALL SELECT 'Inteligencia Artificial', 5
    UNION ALL SELECT 'Ingenier�a de Software II', 5
    UNION ALL SELECT 'Arquitectura de Software', 5
    UNION ALL SELECT 'Ciberseguridad', 5
    UNION ALL SELECT 'Desarrollo de Aplicaciones M�viles', 5

    -- Semestre 6
    UNION ALL SELECT 'Programaci�n Concurrente', 6
    UNION ALL SELECT 'Cloud Computing', 6
    UNION ALL SELECT 'Blockchain y Criptomonedas', 6
    UNION ALL SELECT 'Interacci�n Humano-Computadora', 6
    UNION ALL SELECT 'Gesti�n de Proyectos de Software', 6

    -- Semestre 7
    UNION ALL SELECT 'Internet de las Cosas (IoT)', 7
    UNION ALL SELECT 'DevOps y Automatizaci�n', 7
    UNION ALL SELECT 'Big Data', 7
    UNION ALL SELECT 'Realidad Aumentada y Virtual', 7
    UNION ALL SELECT 'T�picos Selectos en Software Emergente', 7

    -- Semestre 8
    UNION ALL SELECT 'Proyecto Integrador', 8
    UNION ALL SELECT '�tica Profesional en TI', 8
    UNION ALL SELECT 'Innovaci�n y Emprendimiento Tecnol�gico', 8
    UNION ALL SELECT 'Auditor�a y Calidad de Software', 8
    UNION ALL SELECT 'Seminario de Titulaci�n', 8
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
    UNION ALL SELECT '1.2', 'Definici�n y estructura de los patrones de software'
    UNION ALL SELECT '1.3', 'Niveles en los patrones de software'
    UNION ALL SELECT '2.1', 'Definici�n de patr�n arquitect�nico'
    UNION ALL SELECT '2.2', 'Patrones estructurales'
    UNION ALL SELECT '2.3', 'Patrones para sistemas distribuidos'
    UNION ALL SELECT '2.4', 'Patrones para sistemas interactivos'
    UNION ALL SELECT '2.5', 'Patrones para sistemas adaptables'
) C
WHERE M.Nombre = 'Arquitectura de Software' AND Cr.Nombre = 'Software y Tecnologias Emergentes';


INSERT INTO TiposArchivo (Nombre, Extension, MimeType) VALUES
('Documento PDF', '.pdf', 'application/pdf'),
('Presentaci�n PowerPoint', '.pptx', 'application/vnd.openxmlformats-officedocument.presentationml.presentation'),
('Documento Word', '.docx', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'),
('Video MP4', '.mp4', 'video/mp4'),
('Imagen JPEG', '.jpg', 'image/jpeg');


-- Eliminar semestres 1 y 2 de todas las carreras de Ingenier�a excepto Tronco Com�n
DELETE FROM Semestres
WHERE Nombre IN ('Semestre 1', 'Semestre 2')
  AND CarreraId IN (
    SELECT Id FROM Carreras
    WHERE FacultadId = 5 -- Facultad de Ingenier�a
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
