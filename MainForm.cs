/*
 * Создано в SharpDevelop.
 * Пользователь: deski
 * Дата: 04.03.2017
 * Время: 3:45
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using   System.Text.RegularExpressions;


namespace bims
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		OpenFileDialog OPF;
		Bistri b;
		Form frm;
		PictureBox pbZoom;
		FormResult frmR;
		int cur = 0;
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			b = new Bistri();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
	
		void ButtonFilesClick(object sender, EventArgs e)
		{
			if (LoadFiles()) {
				b.wh = 0;
				b.Start();
				UpdatePB();
			}
			
			
	
		}
		bool LoadFiles()
		{
			OPF = new OpenFileDialog();
			OPF.Multiselect = true; 
			OPF.Filter = "image,txt|*.txt;*.jpg;*.bmp;*.png;*.gif|txt files|*.txt";
			if (OPF.ShowDialog() == DialogResult.OK) {				
				foreach (string file in OPF.FileNames) {
					//		MessageBox.Show(file);				
					//if(Path.GetExtension())
					//file.EndsWith("txt")
					string pattern = @".*\.txt$";
					Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
					if (reg.IsMatch(file)) {
						b.LoadTxt(file);
					} else
						b.k.Add(new Node(1, file,b.k.Count));
				}				
				return true;
			}
			return false;
		}
		void ButtonSaveClick(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
			saveFileDialog1.AddExtension = true;
			saveFileDialog1.Filter = "Web files(*.txt)|*.txt|All files(*.*)|*.*";
			if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
				return;			
			b.SaveFiles(textBoxPrefix.Text);
			b.SaveFile(saveFileDialog1.FileName);
			UpdatePB();
	
		}
		public void UpdatePB()
		{
			if (b.k.Count <= 0)
				return;
			if (b.node1.type == 2) {
				pictureBox1.Visible = false;
			richTextBox1.Visible = true;
				richTextBox1.Text = b.node1.str;
			}
			if (b.node2.type == 2) {
				pictureBox2.Visible = false;
				richTextBox2.Visible = true;
				richTextBox2.Text = b.node2.str;
			}
			if (b.node1.type == 1) {
				pictureBox1.Visible = true;
				richTextBox1.Visible = false;
				pictureBox1.ImageLocation = b.node1.str;
				pictureBox1.Load(b.node1.str);
				label1.Text = Path.GetFileName(b.node1.str);
			}
			if (b.node2.type == 1) {
				pictureBox2.Visible = true;
				richTextBox2.Visible = false;
				pictureBox2.ImageLocation = b.node2.str;			
				pictureBox2.Load(b.node2.str);				
				label2.Text = Path.GetFileName(b.node2.str);
			}
			int pr = 0;
			if (b.k.Count != 0)
				pr = b.i * 100 / b.k.Count;
			label3.Text = pr.ToString() + "%";	
		}
		void Button1Click(object sender, EventArgs e)
		{
			b.click1button();
			UpdatePB();
			if (b.i >= b.k.Count && b.k.Count > 0)
				FormResult();
			
		}
		void Button2Click(object sender, EventArgs e)
		{
			b.lick2button();
			UpdatePB();
			if (b.i >= b.k.Count && b.k.Count > 0)
				FormResult();
		}
		void FormResult()
		{
			b.Result();
			frmR = new bims.FormResult(b);
			frmR.Show();
		}
		void CloseFrm(object sender, EventArgs e)
		{
		
			frm.Close();
		}
		
		void PicturesBoxZoom(PictureBox p)
		{
			pbZoom = p;
			frm = new Form();
			frm.KeyPreview = true;
			frm.Click += new System.EventHandler(CloseFrm);	
			frm.KeyDown += new System.Windows.Forms.KeyEventHandler(FrmKeyDown);				
			frm.ControlBox = false;
			frm.FormBorderStyle = FormBorderStyle.None;			
			frm.BackgroundImage = p.Image;
			frm.BackgroundImageLayout = ImageLayout.Zoom;
			Screen scr = Screen.AllScreens.Length > 1 ? Screen.AllScreens[1] : Screen.PrimaryScreen;
			frm.Location = new Point(scr.Bounds.Left, scr.Bounds.Top);
			frm.Size = scr.Bounds.Size;
			frm.BackColor = Color.Black;
			//	frm.Show();			
						
			frm.Show();
		}
		void PictureBox1Click(object sender, EventArgs e)
		{		
			PicturesBoxZoom((PictureBox)sender);					
			cur=0;
			if((PictureBox)sender==pictureBox2)
				cur=b.k.Count-1;
		}
		
		void FrmKeyDown(object sender, KeyEventArgs e)
		{			
			if (e.KeyCode == Keys.Escape)
				frm.Close();
			if (e.KeyCode == Keys.Enter) {
				string pbZoomBefore;
				pbZoomBefore = pbZoom.ImageLocation;
				if (pbZoom == pictureBox1)
					Button1Click(null, null);
				else
					Button2Click(null, null);	
								
				if (b.i >= b.k.Count) {
					frm.Close();				
					return;
				}
				if (b.node1.type == 2 && b.node2.type == 2) {
					frm.Close();				
					return;
				} else if (b.node1.type == 1 && b.node2.type == 2) {
					pbZoom = pictureBox1;
				} else if (b.node1.type == 2 && b.node2.type == 1) {
					pbZoom = pictureBox2;
				} else if (b.node1.type == 1 && b.node2.type == 1) {
					pbZoom = pbZoomBefore == pictureBox1.ImageLocation ? pictureBox2 : pictureBox1;
				}
				frm.BackgroundImage = pbZoom.Image;				

			}
			if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right) {
				if (b.i >= b.k.Count && b.k.Count > 1) {
					//if (b.node1.type == 1)
//						cur = frm.BackgroundImage == pictureBox1.Image ? 0 : cur;
//					//if (b.node2.type == 1)
//						cur = frm.BackgroundImage == pictureBox2.Image ? b.k.Count - 1 : cur;
					if (e.KeyCode == Keys.Left) {
						int i;
						for (i = cur-1; i >= 0; i--) {
							if (b.k[i].type == 1)
								break;							
						}
						if (i < 0) {
							for (i = b.k.Count - 1; i >= cur; i--) {
								if (b.k[i].type == 1)
									break;							
							}
						}
						cur = i;
//						cur = cur == 0 ? b.k.Count - 1 : cur - 1;
					}
					if (e.KeyCode == Keys.Right) {
						int i;
						for (i = cur+1; i <= b.k.Count-1; i++) {
							if (b.k[i].type == 1)
								break;							
						}
						if (i >b.k.Count-1) {
							for (i = 0; i <= cur; i++) {
								if (b.k[i].type == 1)
									break;							
							}
						}
						cur = i;
						//	cur = cur == b.k.Count - 1 ? 0 : cur + 1;
					}
					PictureBox pic = new PictureBox();
					pic.Load(b.k[cur].str);
					frm.BackgroundImage = pic.Image;
					return;					
				}
				if (b.node1.type == 1 && b.node2.type == 1) {
					pbZoom = pbZoom == pictureBox1 ? pictureBox2 : pictureBox1;
					frm.BackgroundImage = pbZoom.Image;				
				}
				
			}

		}
		void MainFormKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1)
				Button1Click(null, null);
			if (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2)
				Button2Click(null, null);
		}
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
	
		}
		void Button3СlickLoadTree(object sender, EventArgs e)
		{
			b.k.Clear();
			if (LoadFiles()) {
				b.i = b.k.Count;
				b.Start();
				UpdatePB();
			}			
		}
		
	}
}
