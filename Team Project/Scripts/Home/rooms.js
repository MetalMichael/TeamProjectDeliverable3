$(document).ready(function() {
    rooms.setup();
    
    $('#TotalStudents').on('mouseUp', function() { rooms.update(); });
    $('#Park').on('change', function() { rooms.update(); });
    $('#Building').on('change', function() { rooms.update(); });
});


var rooms = {
    rooms = null,
    students = null,
    park = null,
    building = null,

    setup = function() {
        var rooms = [];
        $('#Rooms').children().each(function() {
            rooms.push($(this).html().trim());
        });
        rooms.shift();
        self.rooms = rooms;
        
        self.update();
    },
    
    update = function () {
        self.students = $('#StudentsTotal').val();
        self.park = $('#Park').val();
        self.building = $('#Building').val();
        
        self.filterRooms();
    },
    
    filterRooms = function () {
        $.get('/Home/Rooms',
            {students,park,building},
            function (data) {
            
            }
        );
    }
};