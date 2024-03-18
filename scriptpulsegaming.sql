USE [PulseGaming]
GO
/****** Object:  Table [dbo].[Juego]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Juego](
	[IDJuego] [int] NOT NULL,
	[NombreJuego] [nvarchar](100) NOT NULL,
	[IDGenero] [int] NOT NULL,
	[ImagenJuego] [nvarchar](100) NOT NULL,
	[PrecioJuego] [float] NOT NULL,
	[DescripcionJuego] [nvarchar](max) NULL,
	[IDEditor] [int] NOT NULL,
 CONSTRAINT [PK_Juego] PRIMARY KEY CLUSTERED 
(
	[IDJuego] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[V_GRUPO_JUEGOS]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[V_GRUPO_JUEGOS]
as
	select cast(
    ROW_NUMBER() OVER (ORDER BY NombreJuego) as int) AS POSICION
    , IDJuego, NombreJuego, IDGenero, ImagenJuego, PrecioJuego, DescripcionJuego, IDEditor from Juego
GO
/****** Object:  Table [dbo].[DetallesPedido]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetallesPedido](
	[IDDetallePedido] [int] NOT NULL,
	[IDJuego] [int] NULL,
	[Total] [float] NULL,
	[Cantidad] [int] NULL,
	[IDPedido] [int] NULL,
 CONSTRAINT [PK_DetallesPedido] PRIMARY KEY CLUSTERED 
(
	[IDDetallePedido] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Editor]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Editor](
	[IDEditor] [int] NOT NULL,
	[NombreEditor] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Editor] PRIMARY KEY CLUSTERED 
(
	[IDEditor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Genero]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genero](
	[IDGenero] [int] NOT NULL,
	[NombreGenero] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Genero] PRIMARY KEY CLUSTERED 
(
	[IDGenero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pedidos]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pedidos](
	[IDPedido] [int] IDENTITY(1,1) NOT NULL,
	[FechaPedido] [datetime] NOT NULL,
	[Ciudad] [nvarchar](100) NOT NULL,
	[Pais] [nvarchar](100) NOT NULL,
	[IDUsuario] [int] NOT NULL,
	[Total] [float] NOT NULL,
 CONSTRAINT [PK__Pedidos__00C11F99923CE366] PRIMARY KEY CLUSTERED 
(
	[IDPedido] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Plataforma]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Plataforma](
	[IDPlataforma] [int] NOT NULL,
	[NombrePlataforma] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Plataforma] PRIMARY KEY CLUSTERED 
(
	[IDPlataforma] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlataformaJuego]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlataformaJuego](
	[IDPlataformaJuego] [int] NOT NULL,
	[IDPlataforma] [int] NOT NULL,
	[AñoSalida] [int] NOT NULL,
	[IDJuego] [int] NULL,
 CONSTRAINT [PK_PlataformaJuego] PRIMARY KEY CLUSTERED 
(
	[IDPlataformaJuego] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[IDRole] [int] NOT NULL,
	[NombreRole] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[IDRole] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[IDUsuario] [int] NOT NULL,
	[Password] [varbinary](max) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Apellidos] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Telefono] [int] NOT NULL,
	[IDRole] [int] NOT NULL,
	[Salt] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[IDUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Editor] ([IDEditor], [NombreEditor]) VALUES (1, N'Xbox Game Studios')
INSERT [dbo].[Editor] ([IDEditor], [NombreEditor]) VALUES (2, N'Landfall')
INSERT [dbo].[Editor] ([IDEditor], [NombreEditor]) VALUES (3, N'Activision')
INSERT [dbo].[Editor] ([IDEditor], [NombreEditor]) VALUES (4, N'Paradox Interactive')
INSERT [dbo].[Editor] ([IDEditor], [NombreEditor]) VALUES (5, N'Electronic Arts')
INSERT [dbo].[Editor] ([IDEditor], [NombreEditor]) VALUES (6, N'SCS Software')
INSERT [dbo].[Editor] ([IDEditor], [NombreEditor]) VALUES (7, N'Ubisoft')
INSERT [dbo].[Editor] ([IDEditor], [NombreEditor]) VALUES (8, N'Kalypso Media')
INSERT [dbo].[Editor] ([IDEditor], [NombreEditor]) VALUES (9, N'Larian Studios')
INSERT [dbo].[Editor] ([IDEditor], [NombreEditor]) VALUES (10, N'CD Projekt')
INSERT [dbo].[Editor] ([IDEditor], [NombreEditor]) VALUES (11, N'FromSoftware')
INSERT [dbo].[Editor] ([IDEditor], [NombreEditor]) VALUES (12, N'Games Incubator')
INSERT [dbo].[Editor] ([IDEditor], [NombreEditor]) VALUES (13, N'Rockstar Games')
GO
INSERT [dbo].[Genero] ([IDGenero], [NombreGenero]) VALUES (1, N'Shooter')
INSERT [dbo].[Genero] ([IDGenero], [NombreGenero]) VALUES (2, N'Simulacion')
INSERT [dbo].[Genero] ([IDGenero], [NombreGenero]) VALUES (3, N'Carreras')
INSERT [dbo].[Genero] ([IDGenero], [NombreGenero]) VALUES (4, N'Construccion')
INSERT [dbo].[Genero] ([IDGenero], [NombreGenero]) VALUES (7, N'Deporte')
INSERT [dbo].[Genero] ([IDGenero], [NombreGenero]) VALUES (8, N'Fantasia')
INSERT [dbo].[Genero] ([IDGenero], [NombreGenero]) VALUES (9, N'Accion')
GO
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (1, N'Call of Duty Black Ops III', 1, N'bo3.jpg', 59.99, N'Call of Duty: Black Ops III está separado en tres partes, campaña de un jugador que sigue la historia del mundo tras la tercera Guerra Fría, el multijugador y el modo zombi. Este es un Call of Duty futurista que intenta reimaginar cómo podría ser la guerra en el futuro si no hacemos movimientos para asegurarnos de que las tendencias actuales se detengan.', 3)
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (2, N'CITIES SKYLINES II', 4, N'CitiesSkylines.jpg', 49.99, N'Levanta una ciudad del suelo y reimagínala como la bulliciosa metrópolis de tus sueños. Prepárate para una experiencia sin precedentes en desarrollo urbano. Con simulaciones avanzadas y una economía dinámica, Cities: Skylines II ofrece posibilidades ilimitadas para la construcción del mundo. Establece los cimientos de tu ciudad y dale vida. Construye las infraestructuras esenciales, las redes de carreteras y los sistemas vitales que sustentan el funcionamiento diario de la sociedad. El destino del crecimiento de tu ciudad está en tus manos, pero una cuidadosa planificación estratégica es crucial. Cada decisión tiene sus consecuencias. ¿Puedes impulsar las industrias locales y, al mismo tiempo, aprovechar el comercio para estimular la economía? ¿Cómo puedes fomentar zonas residenciales prósperas sin comprometer la vitalidad del centro de la ciudad? ¿Satisfarás las demandas y aspiraciones de tus ciudadanos manteniendo un presupuesto equilibrado?', 4)
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (3, N'Totally Accurate Battle Simulator', 2, N'totalAccurate.jpg', 16.79, N'El código de Totally Accurate Battle Simulator aporta una experiencia de lucha sandbox que seguramente te encantará desde la primera batalla. Desarrollado y publicado por Landfall, el título ofrece ridículas batallas tácticas basadas en la física que abarcan diferentes épocas de nuestra existencia y evolución. ¡Sumérgete en las batallas más intensas que jamás hayas tenido el placer de presenciar enfrentando a enemigos que nunca podrían enfrentarse de otra manera!', 2)
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (4, N'Forza Horizon 5 Premium Edition', 3, N'forzaHorizon.jpg', 99.99, N'El código de Forza Horizon 5 Premium Edition ofrece una oportunidad única de viajar por la tierra de México y disfrutar de los eventos climáticos dinámicos que incluyen intensas tormentas tropicales, y las estaciones cambian semanalmente. Cada nueva zona del mapa desbloquea coleccionables que debes encontrar, eventos en los que participar y nuevos trofeos que conseguir. Lo mejor de todo es que puedes vivir todas las aventuras junto a tus amigos, participar en eventos y en continuas competiciones para ver quién es el mejor cerebro al volante. Si se te ocurre una forma de mejorar las carreras existentes o tienes nuevas ideas creativas, la herramienta de juego EventLab te permite crear acrobacias, desafíos, carreras e incluso crear modos de juego. Compra el código de Forza Horizon 5 Premium Edition para PC/Xbox y participa en uno de los títulos de carreras más grandiosos de todos los tiempos, procedente de la emblemática serie de exclusivos de Xbox.', 1)
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (5, N'Call of Duty: Modern Warfare 3', 1, N'mw3.png', 39.99, N'Call of Duty: Modern Warfare 3 (2011) es la tercera entrega en la saga de CoD Modern Warfare. Este juego tiene la difícil tarea de estar a la altura de CoD: Modern Warfare 2, uno de los mejores CoD de la historia. Con gráficos mejorados, una nueva campaña y el mejor multijugador de su tiempo, los desarrolladores hicieron un buen trabajo.', 1)
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (6, N'EA SPORTS FC 24', 7, N'FC24.jpg', 16.62, N'En el electrizante universo de EA Sports FC 24, ¡tú eres el cerebro! Forma tu Ultimate Team, un campo de juego en el que darás vida a tu equipo de fantasía. Siente el pulso de la emoción en el modo Carrera mientras forjas una odisea futbolística inconfundible. Desvíate a las calles con VOLTA, donde el fútbol urbano manda. Pon a prueba tu temple en los estadios internacionales, donde los torneos mundiales llevan tus habilidades al límite. Prepárate para emocionantes enfrentamientos con jugadores de todo el mundo, forjando amistades, desatando rivalidades y abrazando el espíritu de la competición. EA Sports FC 24 no es un juego más. Es una comunidad vibrante, una plataforma dinámica donde puedes conectar con otros aficionados al fútbol de todo el mundo. Comparte tu pasión, celebra tus victorias y crea recuerdos inolvidables: ¡compra EA Sports FC 24 código de Xbox Live!', 5)
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (7, N'The Sims 4 Digital Deluxe Edition', 2, N'SIMS4.jpg', 24.99, N'Los Sims 4 es el esperadísimo juego de simulación social que permite jugar a la vida como nunca antes. Crea nuevos Sims, con inteligencia y emociones, y comprueba que todas sus acciones son relevantes y se ven afectadas por sus interacciones y estados emocionales.', 5)
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (8, N'F1® 24 Standard Edition', 3, N'F124.jpg', 69.99, N'Prepárate para encender tu pasión por las carreras y acercarte a la parrilla como nunca antes con EA SPORTS™ F1® 24, el videojuego oficial del 2024 FIA Formula One World Championship™. Libera tu espíritu de victoria, lábrate una carrera legendaria en la F1® y logra una sincronía perfecta con tu coche gracias a las últimas novedades en manejo y física proporcionadas por EA SPORTS Dynamic Handling.', 5)
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (9, N'Apex Legends', 1, N'APEX.jpg', 0, N'Apex Legends es un hero shooter para jugar gratis con un grupo bien conectado y en continua expansión de personajes legendarios con poderosas habilidades, repleto de cientos de elementos cosméticos para desbloquear.', 5)
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (10, N'Euro Truck Simulator 2', 2, N'ets2.jpg', 19.99, N'Es una secuela directa del videojuego de 2008 Euro Truck Simulator y es el segundo de la serie. El jugador puede conducir una selección camiones articulados a través de un mundo abierto modelado según el continente de Europa, recogiendo y entregando cargas de distintas empresas.', 6)
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (11, N'The Crew 2', 3, N'thecrew2.jpg', 49.99, N'The Crew 2 es la secuela del juego de conducción online multijugador, que en esta ocasión, además de ofrecer mejores gráficos y modos de juego, nos permite conducir por nuevos medios y superficies. De esta forma, The Crew 2 añade aviones y lanchas, circuitos acuáticos y más opciones de configuración.', 7)
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (12, N'Minecraft', 4, N'minecraft.jpg', 29.99, N'Minecraft es un videojuego tipo sandbox, su traducción literal sería “caja de arena” y es lo que representa la experiencia de juego. Los jugadores pueden modelar el mundo a su gusto, destruir y construir, como si estuviesen jugando en una caja de arena.', 1)
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (13, N'Tropico 3: Gold Edition', 4, N'tropico3.jpg', 15, N'Tropico 3 es un videojuego de construcción y gestión, además de ser de simulación política desarrollado por Haemimont games y publicado por Kalypso Media. Al igual que los otros juegos de la serie Tropico, tiene un fuerte énfasis en la construcción de la ciudad.', 8)
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (14, N'Baldur''s Gate 3', 8, N'baldursgate3.jpg', 59.99, N'El regreso de una de las sagas RPG más queridas por los jugadores de todo el mundo. Baldur''s Gate 3 trae de vuelta el mejor rol de fantasía de la mano de Larian Studios, los autores de la no menos exitosa serie de aventuras roleras Divinity: Original Sin. En este nuevo RPG ambientado en la legendaria marca de Wizard of the Coast te enfrentas a la amenaza de los Azotamentes en la piel de un aventurero al que puedes diseñar con total libertad usando las 12 clases y 46 subclases de personaje disponibles en este juego de PC y PS5. Hay cientos de hechizos y habilidades únicas entre las que elegir para crear al héroe o villano de tus sueño, sintiendo que verdaderamente estás disfrutando de una partida de rol con tablero y dados.', 9)
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (15, N'The Witcher 3: Wild Hunt', 8, N'thewitcher3.jpg', 29.99, N'¡The Witcher 3: Wild Hunt ofrece un RPG mundo abierto de acción que simplemente no puedes dejar pasar! Controla a Geralt de Rivia, también conocido como El Brujo: un cazador altamente entrenado que cuenta con sentidos mejorados y un alto conocimiento de los monstruos y males del mundo.', 10)
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (16, N'Elden Ring', 8, N'eldenring.jpg', 59.99, N'Las Tierras Intermedias, empañadas por la guerra, sólo pueden sentir la gracia de la Gran Voluntad una vez más cuando un nuevo Señor de los Elden blande el Anillo de los Elden. Levántate, Tarnished, y sigue el camino más allá del mar de niebla para encontrar tu destino en la nueva experiencia similar a Dark Souls de FromSoftware Inc. Escrito por Hidetaka Miyazaki, el creador de Dark Souls, y George R.R. Martin, la mente maestra detrás de Canción de Hielo y Fuego, el juego promete un viaje cautivador pero brutal, en el que la valentía, la determinación y la insaciable sed de triunfo son la clave para acabar recogiendo todos los fragmentos del Elden Ring. ¡Compra Elden Ring Código de Steam y desvela todos los misterios de las Tierras Intermedias!', 11)
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (17, N'Zoo Simulator', 2, N'zoosimulator.jpg', 0, N'En Zoo Simulator tu principal tarea es dirigir y gestionar tu propio zoo. ¡Renuévalo y cuida de todas las necesidades de los animales proporcionándoles una vida tranquila! ¿Atraerás a multitudes de visitantes? ¡Crea tu propio zoo!', 12)
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (18, N'Grand Theft Auto V', 9, N'gta5.jpg', 29.99, N'GTA V se ambienta en Los Santos, ciudad ficticia basada en Los Angeles. En cuanto a GTA Online, el multijugador permite hasta 30 jugadores en línea para explorar el mapeado del juego y disputar distintas misiones de forma cooperativa y/o competitiva, además de celebrarse diferentes eventos para mantener la comunidad de GTA V activa. Actualmente, se rumorea que el juego podría tener un DLC de historia (una expansión argumental) protagonizada por un nuevo personaje, aunque de momento Rockstar no se ha pronunciado.', 13)
INSERT [dbo].[Juego] ([IDJuego], [NombreJuego], [IDGenero], [ImagenJuego], [PrecioJuego], [DescripcionJuego], [IDEditor]) VALUES (19, N'Grand Theft Auto VI', 9, N'gta6.jpg', 79.99, N'Grand Theft Auto VI (abreviado como GTA VI) es un próximo videojuego que está siendo desarrollado por Rockstar Games. Será la octava entrega principal de dicha serie, tras Grand Theft Auto V de 2013, y la decimosexta en total. Tras años de especulaciones y filtraciones, Rockstar confirmó que el juego estaba en desarrollo en febrero de 2022. ​Está previsto para el lanzamiento en 2025 para PlayStation 5 y en Xbox Series X|S.', 13)
GO
SET IDENTITY_INSERT [dbo].[Pedidos] ON 

INSERT [dbo].[Pedidos] ([IDPedido], [FechaPedido], [Ciudad], [Pais], [IDUsuario], [Total]) VALUES (1, CAST(N'2024-03-11T18:40:41.523' AS DateTime), N'wdqd', N'qwd', 1, 49.99)
INSERT [dbo].[Pedidos] ([IDPedido], [FechaPedido], [Ciudad], [Pais], [IDUsuario], [Total]) VALUES (2, CAST(N'2024-03-11T18:46:25.630' AS DateTime), N'wdqd', N'feehg', 1, 49.99)
INSERT [dbo].[Pedidos] ([IDPedido], [FechaPedido], [Ciudad], [Pais], [IDUsuario], [Total]) VALUES (3, CAST(N'2024-03-11T18:50:55.690' AS DateTime), N'hola', N'hola', 1, 59.99)
INSERT [dbo].[Pedidos] ([IDPedido], [FechaPedido], [Ciudad], [Pais], [IDUsuario], [Total]) VALUES (4, CAST(N'2024-03-11T18:52:30.370' AS DateTime), N'wdqd', N'feehg', 1, 49.99)
INSERT [dbo].[Pedidos] ([IDPedido], [FechaPedido], [Ciudad], [Pais], [IDUsuario], [Total]) VALUES (5, CAST(N'2024-03-11T19:05:35.580' AS DateTime), N'hola', N'hola', 1, 49.99)
INSERT [dbo].[Pedidos] ([IDPedido], [FechaPedido], [Ciudad], [Pais], [IDUsuario], [Total]) VALUES (6, CAST(N'2024-03-11T19:07:09.550' AS DateTime), N'wdqd', N'qwd', 1, 59.99)
INSERT [dbo].[Pedidos] ([IDPedido], [FechaPedido], [Ciudad], [Pais], [IDUsuario], [Total]) VALUES (7, CAST(N'2024-03-11T19:12:40.217' AS DateTime), N'fe', N'wfw', 1, 49.99)
INSERT [dbo].[Pedidos] ([IDPedido], [FechaPedido], [Ciudad], [Pais], [IDUsuario], [Total]) VALUES (9, CAST(N'2024-03-11T19:13:25.917' AS DateTime), N'ewrgwe', N'wegwe', 1, 49.99)
INSERT [dbo].[Pedidos] ([IDPedido], [FechaPedido], [Ciudad], [Pais], [IDUsuario], [Total]) VALUES (11, CAST(N'2024-03-11T19:14:24.350' AS DateTime), N'tyn', N'tynt', 1, 49.99)
INSERT [dbo].[Pedidos] ([IDPedido], [FechaPedido], [Ciudad], [Pais], [IDUsuario], [Total]) VALUES (12, CAST(N'2024-03-11T19:14:24.350' AS DateTime), N'tyn', N'tynt', 1, 49.99)
INSERT [dbo].[Pedidos] ([IDPedido], [FechaPedido], [Ciudad], [Pais], [IDUsuario], [Total]) VALUES (13, CAST(N'2024-03-18T13:50:51.540' AS DateTime), N'hola', N'hola', 1, 119.98)
INSERT [dbo].[Pedidos] ([IDPedido], [FechaPedido], [Ciudad], [Pais], [IDUsuario], [Total]) VALUES (14, CAST(N'2024-03-18T13:51:44.680' AS DateTime), N'qqq', N'qqq', 1, 0)
SET IDENTITY_INSERT [dbo].[Pedidos] OFF
GO
INSERT [dbo].[Plataforma] ([IDPlataforma], [NombrePlataforma]) VALUES (1, N'XBOX Series X')
INSERT [dbo].[Plataforma] ([IDPlataforma], [NombrePlataforma]) VALUES (2, N'PlayStation 5')
INSERT [dbo].[Plataforma] ([IDPlataforma], [NombrePlataforma]) VALUES (3, N'PlayStation 4')
INSERT [dbo].[Plataforma] ([IDPlataforma], [NombrePlataforma]) VALUES (4, N'Nintendo Switch')
INSERT [dbo].[Plataforma] ([IDPlataforma], [NombrePlataforma]) VALUES (5, N'PC')
GO
INSERT [dbo].[PlataformaJuego] ([IDPlataformaJuego], [IDPlataforma], [AñoSalida], [IDJuego]) VALUES (1, 3, 2015, NULL)
INSERT [dbo].[PlataformaJuego] ([IDPlataformaJuego], [IDPlataforma], [AñoSalida], [IDJuego]) VALUES (2, 5, 2023, NULL)
INSERT [dbo].[PlataformaJuego] ([IDPlataformaJuego], [IDPlataforma], [AñoSalida], [IDJuego]) VALUES (3, 5, 2019, NULL)
INSERT [dbo].[PlataformaJuego] ([IDPlataformaJuego], [IDPlataforma], [AñoSalida], [IDJuego]) VALUES (4, 1, 2021, NULL)
GO
INSERT [dbo].[Roles] ([IDRole], [NombreRole]) VALUES (1, N'Admin')
INSERT [dbo].[Roles] ([IDRole], [NombreRole]) VALUES (2, N'Cliente')
GO
INSERT [dbo].[Usuarios] ([IDUsuario], [Password], [Nombre], [Apellidos], [Email], [Telefono], [IDRole], [Salt]) VALUES (1, 0x557D0D5D8E454AAA7BC23230A09F35120F3C851178F6FF4A6E963F011B3C975242E2B40046AC7A4EFFF953D071847F48CDEBA2764B8FB6B2782EE0678F33B2FC, N'Razvan', N'Lucaci', N'razvan@email.com', 1234567, 1, N'¯Vg4?gè>Än­^fÎüàÛlV?ì·Òà%q_ð Ï^(:½í>]B''Y')
INSERT [dbo].[Usuarios] ([IDUsuario], [Password], [Nombre], [Apellidos], [Email], [Telefono], [IDRole], [Salt]) VALUES (3, 0x7F068D1BB3CBC07230898737601A2E5ACE3833D642A0C0AB3D1C03B695F128E639FAFD91CE9CCC735C569BD0F4BB9A185200B6A4BB3CB7614C36E47F11BDC545, N'Carlos', N'carlos', N'carlos@email.com', 1234567, 2, N'QnB°²[aÅ@Þ5|ëøèÊ
·wäå¹
)¨9`&ù°VVN¼×=Ä¸#áX¾²')
GO
ALTER TABLE [dbo].[DetallesPedido]  WITH CHECK ADD  CONSTRAINT [FK_DetallesPedido_Juego] FOREIGN KEY([IDJuego])
REFERENCES [dbo].[Juego] ([IDJuego])
GO
ALTER TABLE [dbo].[DetallesPedido] CHECK CONSTRAINT [FK_DetallesPedido_Juego]
GO
ALTER TABLE [dbo].[DetallesPedido]  WITH CHECK ADD  CONSTRAINT [FK_DetallesPedido_Pedidos] FOREIGN KEY([IDPedido])
REFERENCES [dbo].[Pedidos] ([IDPedido])
GO
ALTER TABLE [dbo].[DetallesPedido] CHECK CONSTRAINT [FK_DetallesPedido_Pedidos]
GO
ALTER TABLE [dbo].[Juego]  WITH CHECK ADD  CONSTRAINT [FK_Juego_Editor] FOREIGN KEY([IDEditor])
REFERENCES [dbo].[Editor] ([IDEditor])
GO
ALTER TABLE [dbo].[Juego] CHECK CONSTRAINT [FK_Juego_Editor]
GO
ALTER TABLE [dbo].[Juego]  WITH CHECK ADD  CONSTRAINT [FK_Juego_Genero] FOREIGN KEY([IDGenero])
REFERENCES [dbo].[Genero] ([IDGenero])
GO
ALTER TABLE [dbo].[Juego] CHECK CONSTRAINT [FK_Juego_Genero]
GO
ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD  CONSTRAINT [FK_Pedidos_Usuarios] FOREIGN KEY([IDUsuario])
REFERENCES [dbo].[Usuarios] ([IDUsuario])
GO
ALTER TABLE [dbo].[Pedidos] CHECK CONSTRAINT [FK_Pedidos_Usuarios]
GO
ALTER TABLE [dbo].[PlataformaJuego]  WITH CHECK ADD  CONSTRAINT [FK_PlataformaJuego_Juego] FOREIGN KEY([IDJuego])
REFERENCES [dbo].[Juego] ([IDJuego])
GO
ALTER TABLE [dbo].[PlataformaJuego] CHECK CONSTRAINT [FK_PlataformaJuego_Juego]
GO
ALTER TABLE [dbo].[PlataformaJuego]  WITH CHECK ADD  CONSTRAINT [FK_PlataformaJuego_Plataforma] FOREIGN KEY([IDPlataforma])
REFERENCES [dbo].[Plataforma] ([IDPlataforma])
GO
ALTER TABLE [dbo].[PlataformaJuego] CHECK CONSTRAINT [FK_PlataformaJuego_Plataforma]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_Roles] FOREIGN KEY([IDRole])
REFERENCES [dbo].[Roles] ([IDRole])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_Roles]
GO
/****** Object:  StoredProcedure [dbo].[SP_CREATE_EDITOR]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SP_CREATE_EDITOR]
(@NombreEditor nvarchar(100))
as
	DECLARE @NEXTID int
	select @NEXTID = Max(IDEditor) + 1 from Editor
	insert into Editor values(@NEXTID, @NombreEditor)
GO
/****** Object:  StoredProcedure [dbo].[SP_CREATE_GENERO]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SP_CREATE_GENERO]
(@NombreGenero nvarchar(100))
as
	DECLARE @NEXTID int
	select @NEXTID = Max(IDGenero) + 1 from Genero
	insert into Genero values(@NEXTID, @NombreGenero)
GO
/****** Object:  StoredProcedure [dbo].[SP_DELETE_EDITOR]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_DELETE_EDITOR]
(@IDEditor int)
as
	delete from Editor
	where IDEditor = @IDEditor
GO
/****** Object:  StoredProcedure [dbo].[SP_DELETE_GENERO]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_DELETE_GENERO]
(@IDGenero int)
as
	delete from Genero
	where IDGenero = @IDGenero
GO
/****** Object:  StoredProcedure [dbo].[SP_DELETE_JUEGO]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_DELETE_JUEGO]
(@IDJuego int)
as
	delete from juego
	where IDJuego = @IDJuego
GO
/****** Object:  StoredProcedure [dbo].[SP_DETALLES_JUEGO]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_DETALLES_JUEGO]
(@IDJuego int)
as
	select * from Juego where IDJuego = @IDJuego
GO
/****** Object:  StoredProcedure [dbo].[SP_DETALLES_USUARIO]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_DETALLES_USUARIO]
(@IDUsuario int)
as
	select * from Usuarios where IDUsuario = @IDUsuario 
GO
/****** Object:  StoredProcedure [dbo].[SP_FILTRAR_JUEGOS_CATEGORIAS]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SP_FILTRAR_JUEGOS_CATEGORIAS]
(@idgenero int)
as
	select * from Juego
	where Juego.IDGenero=@idgenero
GO
/****** Object:  StoredProcedure [dbo].[SP_GRUPO_JUEGOS]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_GRUPO_JUEGOS]
(@posicion int)
as
	select IDJuego, NombreJuego, IDGenero, ImagenJuego, PrecioJuego, DescripcionJuego, IDEditor
	from V_GRUPO_JUEGOS
	where posicion >= @posicion and posicion < (@posicion + 4)
GO
/****** Object:  StoredProcedure [dbo].[SP_INSERT_DETALLE_PEDIDO]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_INSERT_DETALLE_PEDIDO]
(@IDJuego int, @Total float, @Cantidad int, @IDPedido int)
AS
	declare @nextid int
	select @nextid = MAX(IDDetallePedido) from DetallesPedido
    INSERT INTO DetallesPedido VALUES (@nextid, @IDJuego, @Total, @Cantidad, @IDPedido)
GO
/****** Object:  StoredProcedure [dbo].[SP_INSERT_DETALLES_PEDIDO]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_INSERT_DETALLES_PEDIDO]
(@IDJuego INT, @Total FLOAT, @Cantidad INT, @IDPedido INT)
AS
    INSERT INTO DetallesPedido (IDJuego, Total, Cantidad, IDPedido)
    VALUES (@IDJuego, @Total, @Cantidad, @IDPedido);
GO
/****** Object:  StoredProcedure [dbo].[SP_INSERT_JUEGO]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_INSERT_JUEGO]
(@NombreJuego nvarchar(100), @IDGenero int, @Imagen nvarchar(100), @Precio float, @Descripcion nvarchar(MAX), @IDEditor int)
as
	DECLARE @NEXTID INT
	SELECT @NEXTID = MAX(IDJuego) +1 FROM Juego
	INSERT INTO Juego VALUES (@NEXTID, @NombreJuego, @IDGenero, @Imagen, @Precio, @Descripcion, @IDEditor)
GO
/****** Object:  StoredProcedure [dbo].[SP_INSERT_PEDIDO]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_INSERT_PEDIDO]
(@FechaPedido DATETIME, @Ciudad NVARCHAR(100), @Pais NVARCHAR(100), @IDUsuario INT, @Total FLOAT)
AS
    INSERT INTO Pedidos VALUES (@FechaPedido, @Ciudad, @Pais, @IDUsuario, @Total);
GO
/****** Object:  StoredProcedure [dbo].[SP_MODIFICAR_EDITOR]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_MODIFICAR_EDITOR]
(@IDEditor int, @NombreEditor nvarchar(100))
as
	update Editor set NombreEditor=@NombreEditor
	where IDEditor = @IDEditor
GO
/****** Object:  StoredProcedure [dbo].[SP_MODIFICAR_GENERO]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_MODIFICAR_GENERO]
(@IDGenero int, @NombreGenero nvarchar(100))
as
	update Genero set NombreGenero=@NombreGenero
	where IDGenero = @IDGenero
GO
/****** Object:  StoredProcedure [dbo].[SP_MODIFICAR_JUEGO]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SP_MODIFICAR_JUEGO]
(@IDJuego int, @NombreJuego nvarchar(100), @IDGenero int, @ImagenJuego nvarchar(100), @PrecioJuego float, @Descripcion nvarchar(MAX), @IDEditor int)
as
	update Juego set NombreJuego=@NombreJuego, IDGenero=@IDGenero, ImagenJuego=@ImagenJuego, PrecioJuego=@PrecioJuego, DescripcionJuego=@Descripcion, IDEditor=@IDEditor where IDJuego = @IDJuego
GO
/****** Object:  StoredProcedure [dbo].[SP_TODOS_JUEGOS]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_TODOS_JUEGOS]
as
	select * from Juego
GO
/****** Object:  StoredProcedure [dbo].[SP_TODOS_USUARIOS]    Script Date: 18/03/2024 20:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SP_TODOS_USUARIOS]
as
	select * from Usuarios
GO
