<?php
    //open connection to mysql db
    $connection = mysqli_connect("localhost:3306","archaism_app","DictionaryOfArchaism123","archaism_dictionary") or die("Error " . mysqli_error($connection));

    //fetch table rows from mysql db
    $sql = "select * from dictionary";
    $result = mysqli_query($connection, $sql) or die("Error in Selecting " . mysqli_error($connection));

    //create an array
    $emparray = array('utf8_encode');
    while($row =mysqli_fetch_assoc($result))
    {
        $emparray[] = $row;
    }
    echo json_encode($emparray, JSON_UNESCAPED_UNICODE);

    //close the db connection
    mysqli_close($connection);
?>
