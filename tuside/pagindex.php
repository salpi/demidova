<?php
$dir=$_SERVER['DOCUMENT_ROOT']."/"."filesupload/files/";
$pageall=$_SERVER['HTTP_HOST']."/"."filesupload/files/";
$file = $dir."picasa.html";

//http://test1.ru/pagindex.php?p=8&pagenum=200&pageurl=Z:/home/test1.ru/www/filesupload/files/picasa.html&pageall=http://test1.ru/filesupload/files/picasa.html
//echo  $file;
$p=$_GET['p'];
$pagenum=200;
if(isset($_GET['pagenum']))
	$pagenum=$_GET['pagenum'];
if(isset($_GET['pageurl']))
	$file = $_GET['pageurl'];
if(isset($_GET['pageall']))
	$pageall=$_GET['pageall'];
else
	$pageall=$_SERVER['PHP_SELF']."?pagenum={$pagenum}&pageurl={$file}";
echo "<h1>Страница № {$p}</h1>";
////print_r($_GET);
//phpinfo(32);

//$doc = new DOMDocument();
//$doc->loadHTMLFile($file);

$lines = file($file);

// Осуществим проход массива и выведем номера строк и их содержимое в виде HTML-кода.
$head="";
$num=0;
foreach ($lines as $line) {
	$pos = strpos($line, "<body ");

// Заметьте, что используется ===.  Использование == не даст верного 
// результата, так как 'a' в нулевой позиции.
	$num++;
	if ($pos === false) {
    	//echo "Строка '$findme' не найдена в строке '$mystring1'";
		$head=$head.$line."\n";		
	} else {

		break;    	
	}
   // echo "Строка #<b>{$line_num}</b> : " . htmlspecialchars($line) . "<br />\n";
}
echo $head;
$headline=$num;
//for($i=$num;$i<count($lines);$i++){
//	echo $lines[$i]."\n";
//}
//вывод произвольной части с номером n
$content="<table>";
if(isset($p)){	
	//bar($p);
	barContent($p);	
	$content=$content."</table>";
	/*
	$num+=$pagenum*($p-1);
	for($i=$num;$i<count($lines);$i++){		
		$pos = strpos($lines[$i], "<p ");
		if ($pos === false) {
			continue;
		}		
		$num=$i;
		
		break;
	}
	
	for($i=$num+$pagenum;$i<count($lines);$i++){
		$pos = strpos($lines[$i], "<p ");
		if ($pos === false) {
			$end=$i;
			continue;
		}
		$end=$i-1;
		break;
	}		
	*/
	
	if(($num+$pagenum)>count($lines))
		$end=count($lines);	
}
if(isset($p)===false)
		$end=count($lines);
echo $content;
for($i=$num;$i<$end;$i++){	
	echo $lines[$i]."\n";
}
//вывод подвала с пагинацией

$numpages=bar();
echo "<br><br>";
for($j=1;$j<=$numpages;$j++){
	if($p==$j)
		echo "&nbsp&nbsp<a style=\"font-size:150%;\"href=\"{$_SERVER['PHP_SELF']}?p={$j}&pagenum={$pagenum}&pageurl={$file}\">{$j}</a>"."&nbsp&nbsp";
	else{
		//пагинация с отображением ближайщих страниц 5
		if(abs($p-$j)>=5)
			continue;
		echo "&nbsp&nbsp<a href=\"{$_SERVER['PHP_SELF']}?p={$j}&pagenum={$pagenum}&pageurl={$file}\">{$j}</a>"."&nbsp&nbsp";
	}
}
echo "<br><br>";
echo "&nbsp&nbsp<a href=\"{$pageall}\">Целиком ({$numpages} стр.)</a>"."&nbsp&nbsp";
$pagecontent=$_SERVER['PHP_SELF']."?p=-1&pagenum={$pagenum}&pageurl={$file}";
echo "&nbsp&nbsp<a href=\"{$pagecontent}\">Содержание</a>"."&nbsp&nbsp";
echo "<br><br>";
//ф-ция с оглавлением
function barContent($pa=0){
	global $headline,$pagenum,$lines,$num,$end;
	global $content,$file;
	$j=0;
	$num=$headline;
	$end=$num;	
	while(true){
		$j++;		
		$num=$end+1;
		for($i=$num;$i<count($lines);$i++){		
			$pos = strpos($lines[$i], "<p ");
			if ($pos === false) {
				continue;
			}		
			$num=$i;
			
			break;
		}
		
		for($i=$num+$pagenum;$i<count($lines);$i++){
			$pos = strpos($lines[$i], "<p ");
			if ($pos === false) {
				$end=$i;
				continue;
			}
			$end=$i-1;
			break;
		}		
		if(($num+$pagenum)>count($lines))
			$end=count($lines);		

		if($pa<0){
			$z=$num;
			$l=$num;
			//создание содержания заголовков h1
			for(;$z<=$end;$z++){
				$pos=strpos($lines[$z],"<h1>");//10
				if ($pos === false) {				
					continue;
				}
				else{
					for($l=$z;$l<=$end;$l++){
						$pos=strpos($lines[$l],"</h1>");//10
						if ($pos === false) {				
							continue;
						}
						break;		
					}
					$h1="";
					for($u=$z;$u<=$l;$u++){
						$h1=$h1.$lines[$u];	
					}
					$arrh1="";
					preg_match("!<h1>(.*)</h1>!si",$h1,$arrh1);
					$content=$content."<tr><td><a href=\"{$_SERVER['PHP_SELF']}?p={$j}&pagenum={$pagenum}&pageurl={$file}\">{$j}</a>&nbsp&nbsp</td><td>".$arrh1[1]."</td></tr>\n";
				}
			}
		}
		if($pa>0){
			if($pa==$j)
				break;
		}
		if($end>=count($lines)){			
			break;			
		}			
	}
	return $j;
}
//ф-ция без оглавления
function bar($pa=0){
	global $headline,$pagenum,$lines,$num,$end;
	$j=0;
	$num=$headline;
	$end=$num;	
	while(true){
		$j++;		
		$num=$end+1;
		for($i=$num;$i<count($lines);$i++){		
			$pos = strpos($lines[$i], "<p ");
			if ($pos === false) {
				continue;
			}		
			$num=$i;
			
			break;
		}
		
		for($i=$num+$pagenum;$i<count($lines);$i++){
			$pos = strpos($lines[$i], "<p ");
			if ($pos === false) {
				$end=$i;
				continue;
			}
			$end=$i-1;
			break;
		}		
		if(($num+$pagenum)>count($lines))
			$end=count($lines);			
		if($pa>0){
			if($pa==$j)
				break;
		}
		if($end>=count($lines)){			
			break;			
		}			
	}
	return $j;
}
?>