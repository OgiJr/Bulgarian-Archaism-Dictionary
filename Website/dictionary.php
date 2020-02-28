  <?php
     require "header.php";
 ?>

 <div id="example-table"></div>

 <script>

 $("#example-table").tabulator();

 $("#example-table").tabulator({
   {title:"Name", field:"id", sortable:true, width:200},
   {title:"Progress", field:"word", sortable:true},
   {title:"Gender", field:"synonym", sortable:true},
   {title:"Favourite Color", field:"definition", sortable:false},
  ],
});

$("#example-table").tabulator("setData", "http://archaismdictionary.bg/db.php");

$(window).resize(function(){
  $("#example-table").tabulator("redraw");
});

 </script>

  <?php
      require "footer.php";
  ?>
