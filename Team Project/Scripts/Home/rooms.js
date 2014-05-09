$(document).ready(function () {
    $('#StudentsTotal').keyup(function () { rooms.filterRooms(); });
    $('#Park').change(function () { rooms.updateBuildings(); });
    $('#buildings').change(function () { rooms.filterRooms(); });
    $('#no_rooms').change(function () { rooms.changeRoomTotal(); });

    rooms.changeRoomTotal();
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
        var options = "<option value='0'></option>";
        for (var x in buildings) {
            options += "<option value='" + buildings[x].BuildingID + "'>" + buildings[x].BuildingName + "</option>";
        }
        $('#buildings').html(options);
        this.filterRooms();
    },

    filterRooms: function () {
        console.log("Filtering Rooms");
        this.updateInfo();
        $.get('/Home/Rooms',
            { students: this.students, park: this.park, building: this.building },
            function (data) {
                rooms.roomInfo = data;
                rooms.changeSelects();
                rooms.changeRoomTotal();
            }
        );
    },

    changeRoomTotal: function () {
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
        select += "<select class='room-select' name='Rooms'>";
        select += this.getOptions();
        select += "</select>";
        select += "</div>";
        select += "</div>";
        return select;
    },

    getOptions: function () {
        var options = "<option value='0'></option>";
        for (var x in this.roomInfo) {
            options += "<option value='" + this.roomInfo[x].RoomID + "'>";
            options += this.roomInfo[x].RoomCode + "&nbsp;&nbsp;&nbsp;&nbsp;(Cap:" + this.roomInfo[x].Capacity + ")";
            options += "</option>";
        }
        return options;
    }
};