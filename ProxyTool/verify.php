<?php
/*
This is the validate script that the proxy scanner uses. It requires curl module for PHP.
It would be pretty simple to change this to expect something else. 

All it does is attempts to call its own IP page (which just does : 
echo ($_SERVER["HTTP_CF_CONNECTING_IP"] ? $_SERVER["HTTP_CF_CONNECTING_IP"] :  $_SERVER[ "REMOTE_ADDR" ] );
 ). Then it takes the output IP and checks it against the IP it has. 

 */

$res = call_("http://dab.biz/ip.php", $_GET['proxy']);

//var_dump($res);
$ip = explode(':', $_GET['proxy'])[0];

if ($res == $ip || $res == gethostbyname($ip))
{
	die('true');

}

die('false');

function call_($url, $proxy)
{
        //echo "NAVIGATING TO " . $url . "\n";
        $ch = curl_init();
        curl_setopt($ch, CURLOPT_URL,$url);
        curl_setopt($ch, CURLOPT_PROXY, $proxy);
        curl_setopt($ch, CURLOPT_FOLLOWLOCATION, 1);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
        curl_setopt($ch, CURLOPT_COOKIESESSION, true); // ignore any previous cookies
        curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false); // Don't validate SSL Cert
        curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, false); // Don't validate SSL Cert
        //curl_setopt($ch, CURLOPT_HEADER, 1);
        $re = curl_exec($ch);
        curl_close($ch);
        return $re;

}



?>

