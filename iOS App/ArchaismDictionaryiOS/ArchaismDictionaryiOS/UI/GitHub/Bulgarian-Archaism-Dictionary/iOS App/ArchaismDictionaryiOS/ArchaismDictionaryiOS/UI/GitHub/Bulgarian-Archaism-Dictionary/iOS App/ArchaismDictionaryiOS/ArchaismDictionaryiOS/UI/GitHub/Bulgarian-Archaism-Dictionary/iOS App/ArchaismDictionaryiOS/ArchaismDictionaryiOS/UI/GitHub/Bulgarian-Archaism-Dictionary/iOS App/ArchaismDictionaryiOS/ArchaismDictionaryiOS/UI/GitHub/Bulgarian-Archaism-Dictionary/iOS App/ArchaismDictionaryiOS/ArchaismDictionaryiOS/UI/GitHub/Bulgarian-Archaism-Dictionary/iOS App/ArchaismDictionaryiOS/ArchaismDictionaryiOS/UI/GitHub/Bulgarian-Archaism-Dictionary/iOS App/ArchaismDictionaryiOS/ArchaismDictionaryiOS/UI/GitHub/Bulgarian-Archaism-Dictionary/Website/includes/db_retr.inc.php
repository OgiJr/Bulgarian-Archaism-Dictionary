<?php

$sql=mysql_query("select * from food_breakfast");

   while($row=mysql_fetch_assoc($sql))
   {
   $ID=$row['ID'];
   $Consumption=$row['Consumption'];
   $Subline=$row['Subline'];
   $Price=$row['Price'];
   $visible=$row['visible'];

   $posts[] = array('ID'=> $ID, 'Consumption'=> $Consumption, 'Subline'=> $Subline, 'Price'=> $Price, 'visible'=> $visible);
   }
   $response['posts'] = $posts;

   $fp = fopen('results.json', 'w');
   fwrite($fp, json_encode($response));
   fclose($fp);

   ?>
