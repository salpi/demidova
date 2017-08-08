<?php
$dir=$_SERVER['DOCUMENT_ROOT']."/"."filesupload/files/";
$file = $dir."picasa.html";
//echo  $file;
$p=$_GET['p'];
$pagenum=200;
if(isset($_GET['pagenum']))
	$pagenum=$_GET['pagenum'];
if(isset($_GET['pageurl']))
	$file = $_GET['pageurl'];
echo "<h1>Страница № {$p}</h1>";
//print_r($_GET);
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

if(isset($p)){	
	bar($p);
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

for($i=$num;$i<$end;$i++){
	echo $lines[$i]."\n";
}
//вывод подвала с пагинацией

$numpages=bar();
for($j=1;$j<=$numpages;$j++){
	if($p==$j)
		echo "&nbsp&nbsp<a style=\"font-size:150%;\"href=\"{$_SERVER['PHP_SELF']}?p={$j}&pagenum={$pagenum}&pageurl={$file}\">{$j}</a>"."&nbsp&nbsp";
	else
		echo "&nbsp&nbsp<a href=\"{$_SERVER['PHP_SELF']}?p={$j}&pagenum={$pagenum}&pageurl={$file}\">{$j}</a>"."&nbsp&nbsp";
}
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