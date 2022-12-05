var dataTable;
$(document).ready(function () {
    
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAllProducts"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "isbn", "width": "10%" },
            { "data": "price", "width": "10%" },
            { "data": "author", "width": "15%" },
           /* { "data": "imageURL", "width": "15%" },*/
            { "data": "categoryName", "width": "10%" },
            { "data": "coverTypeName", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                     <div class="w-60 btn-group" role="group">
                        <a href=/Admin/Product/Edit?id=${data} 
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i>Edit</a>

                        <a href=/Admin/Product/Delete?id=${data}
                        class="btn btn-primary mx-2"> <i class="bi bi-trash-fill"></i>Delete </a>
                     </div>
                    `


                },

                "width":"15%"


             }
            
            

            
        ]  

    });
}