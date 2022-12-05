var dataTable;
$(document).ready(function () {

    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAllCompany"
        },
        "columns": [
            { "data": "name", "width": "10%" },
            { "data": "streetAddress", "width": "10%" },
            { "data": "city", "width": "10%" },
            { "data": "state", "width": "10%" },
            { "data": "postalCode", "width": "10%" },
            { "data": "phoneNumber", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                     <div class="w-60 btn-group" role="group">
                        <a href=/Admin/Company/Edit?id=${data} 
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i>Edit</a>

                        <a href=/Admin/Company/Delete?id=${data}
                        class="btn btn-primary mx-2"> <i class="bi bi-trash-fill"></i>Delete </a>
                     </div>
                    `


                },

                "width":"15%"


             }
            
            

            
        ]  

    });
}