/*
 * Создано в SharpDevelop.
 * Пользователь: deski
 * Дата: 02.03.2017
 * Время: 22:23
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Windows.Forms;


namespace ency
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{			
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Form mf=new MainForm(args);
			Application.Run(mf);
		}
		
	}
}
