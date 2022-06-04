using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EstructuraDeDatos5
{
    internal class UsuarioAdministrador : Usuario
    {
		protected List<Producto> _producto;

		public List<Producto> Producto
		{
			get { return this._producto; }
			set { this._producto = value; }
		}

		public UsuarioAdministrador(string nombre, List<Producto> producto) : base(nombre)
		{
			this._producto = producto;
		}

		public void MenuAdministrador(List<Producto> producto)
		{

			Producto = producto;

			int opcion;
			do
			{

				Console.Clear();
				Console.WriteLine(" Bienvenido Usuario: *" + Nombre + "* ");
				opcion = Validador.PedirIntMenu("\n Menú de Registro de nuevos Productos: " +
									   "\n [1] Crear Producto" +
									   "\n [2] Grabar Producto" +
									   "\n [3] Leer Producto" +
									   "\n [4] Salir del Sistema.", 1, 4);

				switch (opcion)
				{
					case 1:
						DarAltaProducto();
						break;
					case 2:
						GrabarProducto();
						break;
					case 3:
						LeerProducto();
						break;
					case 4:

						break;

				}
			} while (opcion != 4);
		}

		public int BuscarProductoCodigo(string codigo)
		{
			for (int i = 0; i < this._producto.Count; i++)
			{
				if (this._producto[i].Codigo == codigo)
				{
					return i;
				}
			}
			/* si no encuentro el producto retorno una posición invalida */
			return -1;
		}

		Dictionary<string, Producto> productoLista = new Dictionary<string, Producto>();

		protected override void DarAltaProducto()
		{

			string codigo;
			string nombre;
			string descripcion;
			
			int costo;
			int precio;
			int componente;

			string opcion;

			Console.Clear();
			codigo = Validador.PedirCaracterString(" Ingrese el Código del Producto" +
											  "\n El documento debe estar entre este rango.", 6, 6);
			if (BuscarProductoCodigo(codigo) == -1)
			{
				VerPersona();
				Console.WriteLine("\n ¡En hora buena! Puede utilizar este Nombre para crear una Persona Nueva en su agenda");
				nombre = Validador.PedirCaracterString("\n Ingrese el nombre del Producto", 1, 15);
				Console.Clear();
				descripcion = Validador.PedirCaracterString("Ingrese la descripción del Producto", 1, 200);
				Console.Clear();
				costo = Validador.PedirIntMayor("\n Ingrese el Costo",0);

				precio = Validador.PedirIntMayor("Ingrese el precio del Producto",0);
				componente = Validador.PedirIntMayor("Ingrese los Componentes del Producto",0);
				

				opcion = ValidarSioNoPersonaNoCreada("\n Está seguro que desea crear este producto? ", codigo, nombre);

				if (opcion == "SI")
				{
					Producto p = new Producto(codigo, nombre, descripcion, costo, precio,componente);
					AddPersona(p);
					productoLista.Add(codigo, p);
					VerPersona();
					VerPersonaDiccionario();
					Console.WriteLine("\n Producto con Nombre *" + nombre + "* agregado exitósamente");
					Validador.VolverMenu();
				}
				else
				{
					VerPersona();
					Console.WriteLine("\n Como puede verificar no se creo ningún Producto");
					Validador.VolverMenu();

				}

			}
			else
			{
				VerPersona();
				Console.WriteLine("\n Usted digitó el Código *" + codigo + "*");
				Console.WriteLine("\n Ya existe un Producto con ese código");
				Console.WriteLine("\n Será direccionado nuevamente al Menú para que lo realice correctamente");
				Validador.VolverMenu();

			}

		}

		public void AddPersona(Producto persona)
		{
			this._producto.Add(persona);
		}


		protected override void GrabarProducto()
		{
			using (var archivoLista = new FileStream("archivoLista.txt", FileMode.Create))
			{
				using (var archivoEscrituraAgenda = new StreamWriter(archivoLista))
				{
					foreach (var persona in productoLista.Values)
					{

						var linea =
									"\n Código del Producto: " + persona.Codigo +
									"\n Nombre del Producto: " + persona.Nombre +
									"\n Descripcion del Producto: " + persona.Descripcion +
									"\n Costo del Producto: " + persona.Costo +
									"\n Precio del Producto: " + persona.Precio +
									"\n Componente del Producto: " + persona.Componente;

						archivoEscrituraAgenda.WriteLine(linea);

					}

				}
			}
			VerPersona();
			Console.WriteLine("Se ha grabado los datos de los Productos correctamente");
			Validador.VolverMenu();

		}

		protected override void LeerProducto()
		{
			Console.Clear();
			Console.WriteLine("\n Productos: ");
			using (var archivoLista = new FileStream("archivoLista.txt", FileMode.Open))
			{
				using (var archivoLecturaAgenda = new StreamReader(archivoLista))
				{
					foreach (var persona in productoLista.Values)
					{


						Console.WriteLine(archivoLecturaAgenda.ReadToEnd());


					}

				}
			}
			Validador.VolverMenu();

		}


		protected string ValidarSioNoPersonaNoCreada(string mensaje, string codigo, string nombre)
		{

			string opcion;
			bool valido = false;
			string mensajeValidador = "\n Si esta seguro de ello escriba *" + "si" + "* sin los asteriscos" +
									  "\n De lo contrario escriba " + "*" + "no" + "* sin los asteriscos";
			string mensajeError = "\n Por favor ingrese el valor solicitado y que no sea vacio. ";

			do
			{
				VerPersona();

				Console.WriteLine(
								  "\n Codigo del Producto a Crear: " + codigo +
								  "\n Nombre del Producto a Crear: " + nombre );

				Console.WriteLine(mensaje);
				Console.WriteLine(mensajeError);
				Console.WriteLine(mensajeValidador);
				opcion = Console.ReadLine().ToUpper();
				string opcionC = "SI";
				string opcionD = "NO";

				if (opcion == "" || (opcion != opcionC) & (opcion != opcionD))
				{
					continue;

				}
				else
				{
					valido = true;
				}

			} while (!valido);

			return opcion;
		}


		protected string ValidarStringNoVacioNombre(string mensaje)
		{

			string opcion;
			bool valido = false;
			string mensajeValidador = "\n Por favor ingrese el valor solicitado y que no sea vacio.";


			do
			{
				VerPersona();
				Console.WriteLine(mensaje);
				Console.WriteLine(mensajeValidador);

				opcion = Console.ReadLine().ToUpper();

				if (opcion == "")
				{

					Console.Clear();
					Console.WriteLine("\n");
					Console.WriteLine(mensajeValidador);

				}
				else
				{
					valido = true;
				}

			} while (!valido);

			return opcion;
		}

		public void VerPersona()
		{
			Console.Clear();
			Console.WriteLine("\n Productos");
			Console.WriteLine(" #\t\tCódigo.\t\tNombre.\t\tDescripción.");
			for (int i = 0; i < Producto.Count; i++)
			{
				Console.Write(" " + (i + 1));

				Console.Write("\t\t");
				Console.Write(Producto[i].Codigo);
				Console.Write("\t\t");
				Console.Write(Producto[i].Nombre);
				Console.Write("\t\t");
				Console.Write(Producto[i].Descripcion);
				Console.Write("\t\t");

				Console.Write("\n");
			}

		}

		public void VerPersonaDiccionario()
		{
			Console.WriteLine("\n Productos en el Diccionario");
			for (int i = 0; i < productoLista.Count; i++)
			{
				KeyValuePair<string, Producto> persona = productoLista.ElementAt(i);

				Console.WriteLine("\n Código de Producto: " + persona.Key);
				Producto personaValor = persona.Value;


				Console.WriteLine(" Nombre del Producto: " + personaValor.Nombre);
				Console.WriteLine(" Descripción del Producto: " + personaValor.Descripcion);
				Console.WriteLine(" Costo del Producto: " + personaValor.Costo);
				Console.WriteLine(" Precio del Producto: " + personaValor.Precio);
				Console.WriteLine(" Componentes del Producto: " + personaValor.Componente);


			}


		}
	}
}
