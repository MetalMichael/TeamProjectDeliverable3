$(document).ready(function() {
    rooms.setup();
    
    $('#StudentsTotal').keyup( function() { rooms.update(); });
    $('#Park').change( function() { rooms.update(); });
    $('#Building').change( function() { rooms.update(); });
    $('#no_rooms').change( function() { rooms.changeRoomTotal(); });
});


var rooms = {
    rooms: null,
    roomSelect: null,
    students: null,
    park: null,
    building: null,
    roomTotal: 0,

    setup: function() {
        this.roomSelect = $('#room1')[0].outerHTML;
        this.changeRoomTotal();
    
        var rooms = [];
        $('#Rooms').children().each(function() {
            rooms.push($(this).html().trim());
        });
        rooms.shift();
        this.rooms = rooms;
        
        this.update();
    },
    
    update: function () {
        this.students = $('#StudentsTotal').val();
        this.park = $('#Park').val();
        this.building = $('#Building').val();
        
        this.filterRooms();
    },
    
    filterRooms: function () {
        $.get('/Home/Rooms',
            {students:this.students,park:this.park,building:this.building},
            function (data) {
            
            }
        );
    },
    
    changeRoomTotal: function() {
        var newTotal = $('#no_rooms').val();
        console.log('Old: ' + this.roomTotal);
        console.log('New ' + newTotal);
        if(newTotal < this.roomTotal) {
            while(this.roomTotal > newTotal) {
                this.removeRoom();
                this.roomTotal--;
            }
        } else {
            while(this.roomTotal < newTotal) {
                this.addRoom(parseInt(this.roomTotal) +1);
                this.roomTotal++;
            }
        }
    },
    
    removeRoom: function() {
        $('#room-container .field').last().remove();
    },
    
    addRoom: function(id) {
        var select = $(this.roomSelect).prop('id', 'room' + id);
        $('#room-container').append(select);
        $('#room' + id).show();
    }
};