/*
 * Создано в SharpDevelop.
 * Пользователь: deski
 * Дата: 04.03.2017
 * Время: 8:27
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using   System.Text.RegularExpressions;

namespace bims
{
	/// <summary>
	/// type - 1-url || 2-text; str - container 
	/// </summary>
	public class Node
	{
		public int type;
		public string str;
		public string result;
		public int loadNumber;
		public Node(int type = 1, string str = "",int n=-1)
		{
			this.type = type;
			this.str = str;
			loadNumber=n;
		}
	}
	/// <summary>
	/// Description of Bistri.
	/// </summary>
	public class Bistri
	{
		public List<Node> k;
		public Node node1;
		public Node node2;
		int m = 0;
		public int i = 1;
		int l = 0;
		int r = 0;
		public int wh = 0;
		Node x = null;
		public string result;
    	
		public Bistri()
		{
			k = new List<Node>();			
		}
		public int nextEl()
		{
			if (k.Count <= 0)
				return 0;

			if (i >= k.Count) {
				firstLast();
				return 0;
			}
	
			if (this.wh == 0) {
				x = k[i]; //{запомним элемент}
				l = 0; //{левый край}
				r = i - 1; //{правый}
				this.wh = 1;
			}
			//while (l <= r) { // {пока левый не больше правого}
			if (l <= r) {
				m = (l + r) / 2; //{находим середину}	            
				node1 = x;
				node2 = k[m];	
				return 1;
			}
			this.wh = 0;
			for (int j = i - 1; j >= l; j--) {
				k[j + 1] = k[j];
			} //{сдвигаем массив вправо на 1}
			k[l] = x; //{вставляем элемент на место}
			this.i++;
			return 1;
		}
		//Для силовой последовательности
		//		public void runProc(){
		//        //int k;
		//	        int n = k.Capacity;
		//	        //{сортировка бинарными вставками}
		//	        num = 0;
		//	        for (var i = 1; i < n; i++) {
		//	            x = k[i]; //{запомним элемент}
		//	            l = 0; //{левый край}
		//	            r = i - 1; //{правый}
		//	            while (l <= r) { // {пока левый не больше правого}
		//	                m=(l + r) / 2; //{находим середину}
		//	                num++;
		//	                if (x < k[m]) r = m - 1; //{если элемент меньше среднего,    правый край левее середины}
		//	                else l = m + 1; //{иначе левый правее середины}
		//	            }
		//	            for (int j = i - 1; j >= l; j--) {
		//	                k[j + 1] = k[j];
		//	            } //{сдвигаем массив вправо на 1}
		//	            k[l] = x; //{вставляем элемент на место}
		//	        }
		//    	}
		void firstLast()
		{
			if (k.Count <= 0)
				return;
			node1 = k[0];
			node2 = k[k.Count - 1];			
		}

		public void click1button()
		{
			r = m - 1;
			if (nextEl() > 0)
				nextEl();
			//else firstLast();
		}

		public void lick2button()
		{
			l = m + 1;
			if (nextEl() > 0)
				nextEl();
			//else firstLast();
		}
		public void Start()
		{
			nextEl();
		}
		public int maxdif()
		{
			int r = k.Count;
			int l = 1;
			int sum = 0;
			for (int i = l; i < r; i++) {
				sum += 2 * (r - i);
				r--;
			}
		
			return sum;
		}
		public void Result()
		{ 
			int i;
			int mod = 0;
			int sumdif = 0;
			for (int j = 0; j < k.Count; j++) {
				for (i = 0; i < k.Count; i++) {
					if (k[i].str == k[j].str) {
						mod = Math.Abs(k[i].loadNumber - j);
						sumdif += mod;
						break;
					}
				}
				if (mod > 0){					
					k[j].result =(j+1).ToString() + "!="+(k[j].loadNumber+1).ToString()+" " + k[j].str;
				}
				else{					
					k[j].result ="+" + (j+1).ToString() + " " + k[j].str;
				}
    	
			}
			result = "Идентичны " + (maxdif() - sumdif).ToString() + "/" + (maxdif()).ToString();
		}
		public void LoadTxt(string filename)
		{
			string line;
			bool isFile = false;
			string pattern = @"([^\s]+(\.(jpg|png|gif|bmp))$)";		
			Regex reg;
			Uri u;
			string text = "";
			
			StreamReader sr = new StreamReader(filename,System.Text.Encoding.UTF8);
			while ((line = sr.ReadLine()) != null) {				
				if (Uri.TryCreate(line, UriKind.Absolute, out u)) {					
					reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
					if (reg.IsMatch(line))
						isFile = true;						
					else
						isFile=false;
				}				
				if (System.IO.File.Exists(line)) {
					k.Add(new Node(1, line,k.Count));
					continue;
				} else if (Uri.IsWellFormedUriString(line, UriKind.Absolute) && isFile) {										
					k.Add(new Node(1, line,k.Count));
					continue;					
				} else {
					text = "";
					if (!line.StartsWith("------"))
						text += line + "\n";
					while ((line = sr.ReadLine()) != null) {
						if (line.StartsWith("------")) {
							reg = new Regex(@"^[\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
							if (reg.IsMatch(text)){
								break;
							}							
							k.Add(new Node(2, text,k.Count));
							break;
						}						
						text += line + "\n";							
					}																				
				}
					      
			}
			sr.Close();
		}
		public void SaveFile(string filename)
		{	
			using (StreamWriter sw = new StreamWriter(filename, false, System.Text.Encoding.UTF8)) {        			
				for (int i=0;i<k.Count;i++) {
					if(k[i].type==2){
						sw.WriteLine("--------------");
						sw.Write(k[i].str);
						if((i+1<k.Count)&&k[i+1].type==1)
							sw.WriteLine("--------------");
					}else if(k[i].type==1){
						sw.WriteLine(k[i].str);
					}
				}				
				sw.WriteLine("--------------");
				sw.Close();
			}
		}
		public void SaveFiles(string pr)
		{
			pr = pr.Trim();
			string fname, prline, spath, newname;
			if (pr.Length < 1)
				return;				
			for (int i = 0; i < k.Count; i++) {
				if (k[i].type == 1) {
					if (!System.IO.File.Exists(k[i].str))
						continue;
					fname = Path.GetFileName(k[i].str);
					spath = k[i].str.Substring(0, k[i].str.LastIndexOf(fname));
					fname = fname.Substring(fname.IndexOf("_") + 1);
					prline = pr + (i + 1).ToString();
					prline = prline.Substring(prline.Length - pr.Length);					
					newname = spath + prline + "_" + fname;
					if (k[i].str == newname)
						continue;
					File.Move(k[i].str, newname);
					k[i].str = newname;
				}
			}
			this.i = k.Count;
			firstLast();		
		}
	}
	
}