$(document).ready(function () {
    $('#StudentsTotal').keyup(function () { rooms.filterRooms(); });
    $('#Park').change(function () { rooms.updateBuildings(); });
    $('#buildings').change(function () { rooms.filterRooms(); });
    $('#no_rooms').change(function () { rooms.changeRoomTotal(); });

    if (typeof selectedRooms !== "undefined") {
        var setup = true;
        $('#no_rooms').val(selectedRooms.length);
    } else {
        var setup = false;
    }
    rooms.filterRooms(setup);
});

var rooms = {
    rooms: null,
    roomInfo: null,
    students: null,
    park: null,
    building: null,
    roomTotal: 0,

    updateInfo: function () {
        this.students = $('#StudentsTotal').val();
        this.park = $('#Park').val();
        this.building = $('#buildings').val();
    },

    updateBuildings: function () {
        this.updateInfo();
        $.get('/Home/Buildings',
            { park: this.park },
            function (data) {
                rooms.changeBuildings(data);
            }
        );
    },

    changeBuildings: function (buildings) {
        $('#buildings').html(buildings);
        this.filterRooms();
    },

    filterRooms: function (setup) {
        this.updateInfo();
        $.get('/Home/Rooms',
            { students: this.students, park: this.park, building: this.building },
            function (data) {
                rooms.roomInfo = data;
                rooms.changeSelects();
                rooms.changeRoomTotal(setup);
            }
        );
    },

    changeRoomTotal: function (setup) {
        var newTotal = $('#no_rooms').val();
        if (newTotal < this.roomTotal) {
            while (this.roomTotal > newTotal) {
                this.removeRoom();
                this.roomTotal--;
            }
        } else {
            while (this.roomTotal < newTotal) {
                this.addRoom(parseInt(this.roomTotal) + 1);
                this.roomTotal++;
            }
        }
        if (setup) {
            var x = 1;
            for (var r in selectedRooms) {
                $('#room' + x + " select").val(selectedRooms[r]);
                x++;
            }
        }
    },

    changeSelects: function () {
        $('.room-select').each(function () {
            var val = $(this).val();
            $(this).html(rooms.getOptions());
            $(this).val(val);
        });
    },

    removeRoom: function () {
        $('#room-container .field').last().remove();
    },

    addRoom: function (id) {
        var select = $(this.createSelect()).prop('id', 'room' + id);
        $('#room-container').append(select);
        $('#room' + id).show();
    },

    createSelect: function () {
        var select = "<div class='field'>";
        select += "<div class='editor-label'>";
        select += "<label>Select Room</label>";
        select += "</div>";
        select += "<div class='editor-field'>";
        select += this.roomInfo;
        select += "</div>";
        select += "</div>";
        return select;
    },

    getOptions: function () {
        return $(this.roomInfo).children();
    }
};