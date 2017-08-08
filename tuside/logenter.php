<?php
//Пишет лог, вызывается вставкой в login.php
function InsertIp(){
	global $_SERVER;
	
	$right_now = date("H:i:s;;d.m.y",mktime(date('H')+3,date('i'),date('s'),date('m'),date('d'),date('Y')));
	$piecesCur=explode(";;", $right_now);
	$ra=$_SERVER["REMOTE_ADDR"];
	
    $lines = file("log.cvs");
    $ar=array();
    //print_r($lines);
    $ar[]=$ra.";;".$right_now."\n";
    $i=1;
    for($j=count($lines)-1;$j>=0;$j--){
    	if($lines[$j]=="")
    		continue;
    	$pieces = explode(";;", $lines[$j]);
    	if($pieces[0]===$ra){

    		$i++;    		
    		if($pieces[2]===$piecesCur[1]."\n"){ 
    			continue;
    		}
    		if($i>10)
    			continue;    		
    	}
    	$ar[]=$lines[$j];
    }
    $fd = fopen("log.cvs", 'w');
    foreach($ar as $line)
    	fwrite($fd, $line);
    fclose($fd);  
}
InsertIp();
?>