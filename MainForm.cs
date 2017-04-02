/*
 * Создано в SharpDevelop.
 * Пользователь: deski
 * Дата: 02.03.2017
 * Время: 22:23
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace ency
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		string[] args;
		string[] readText;
		string[] htmltegs;
		public MainForm(string[] args)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			string s = "Пишу по-русски";
			this.args=args;
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void Button1Click(object sender, EventArgs e)
		{
			Stream myStream = null;
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			openFileDialog1.InitialDirectory = "c:\\";
			openFileDialog1.Filter = "Txt Files (*.txt)|*.txt|All files (*.*)|*.*";
			openFileDialog1.FilterIndex = 1;
			openFileDialog1.RestoreDirectory = true;
			
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				try {
					webBrowser1.Url = new Uri(openFileDialog1.FileName);
					if ((myStream = openFileDialog1.OpenFile()) != null) {
						
						using (myStream) {
							// Insert code to read the stream here.
						}
						ReadFile(openFileDialog1.FileName);
						
					}
				} catch (Exception ex) {
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}
			}
		}
		void ReadFile(string filename){
			try {
			readText = File.ReadAllLines(filename);
						int j = 0;
						for(int i=0;i<readText.Length;i++)
							readText[i]=readText[i].Replace("script","_script_");
						
						for (int i = 0; i < readText.Length; i++) {
							if (readText[i].Contains("------")) {
								for (i++; readText[i].Contains("------") || readText[i] == ""; i++)
									;
								j++;
							}
						}
						htmltegs = new string[j];
						j = -1;
						for (int i = 0; i < readText.Length; i++) {
							if (readText[i].Contains("------")) {
								for (i++; readText[i].Contains("------") || readText[i] == ""; i++)
									;
								j++;
								//htmltegs[j]="<p><a href=\"#"+j.ToString()+"\">"+j.ToString()+"</a>"+readText[i]+"</p>";
								string ts = readText[i].Substring(0, readText[i].Length > 120 ? 120 : readText[i].Length);
								htmltegs[j] = string.Format("<a href=\"#p{0}\">{0}  </a>{1}<br>\n", j,ts);
								readText[i] = string.Format("<a name=\"p{0}\"></a>{1}\n", j,readText[i]);
							}
						}
						for(int i=0;i<readText.Length;i++)
							readText[i]=readText[i]+"<br>\n";
						} catch (Exception ex) {
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}		
		}
		void Button2Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
			saveFileDialog1.AddExtension = true;
			saveFileDialog1.Filter = "Web files(*.html)|*.html|All files(*.*)|*.*";
			if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
				return;
			// получаем выбранный файл
			string filename = saveFileDialog1.FileName;
			// сохраняем текст в файл
			//System.IO.File.WriteAllText(filename, "terst");
			SaveFile(filename);
        
			MessageBox.Show("Файл сохранен");
			webBrowser1.Url = new Uri(filename);
		}
		void SaveFile(string filename){
			using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)) {
				byte[] writeBytes;
				int j=1;
				foreach (string line in htmltegs) {
					if(j%2>0)
						writeBytes = Encoding.UTF8.GetBytes("<div style=\"background: #D9FFAD;\">"+line+"</div>"); 
					else
						writeBytes = Encoding.UTF8.GetBytes(line);
					j++;
					file.Write(writeBytes, 0, writeBytes.Length);
				}				
				//writeBytes = Encoding.UTF8.GetBytes("<p>"+"\n");                   
				//file.Write(writeBytes, 0, writeBytes.Length);
				foreach (string line in readText) {
					writeBytes = Encoding.UTF8.GetBytes(line);                   
					file.Write(writeBytes, 0, writeBytes.Length);
				}
				//writeBytes = Encoding.UTF8.GetBytes("</p>"+"\n");                   
				//file.Write(writeBytes, 0, writeBytes.Length);
			}
		}
		void MainFormLoad(object sender, EventArgs e)
		{
			if(args.Length<2)
				return;
			ReadFile(args[0]);			
			SaveFile(args[1]);
			webBrowser1.Url = new Uri(args[1]);
		}
		
	}
}
