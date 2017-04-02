/*
 * Создано в SharpDevelop.
 * Пользователь: deski
 * Дата: 04.03.2017
 * Время: 23:27
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace bims
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class FormResult : Form
	{
		
		public FormResult(Bistri b)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			string res="";	
			res=b.result+"\n\n----------\n";
			for(int i=0;i<b.k.Count;i++){				
				if(b.k[i].type==2)
					res+="----------\n";				
				res+=b.k[i].result+"\n";
				if((b.k[i].type==2)&&((i+1)<b.k.Count)&&(b.k[i+1].type==1))
					res+="----------\n";
			}
			res+="----------\n";
			this.richTextBox1.Text=res;			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
	}
}
