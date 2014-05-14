$(document).ready(function () {
    $('#StudentsTotal').keyup(function () { rooms.filterRooms(); });
    $('#Park').change(function () { rooms.updateBuildings(); });
    $('#buildings').change(function () { rooms.filterRooms(); });
    $('#no_rooms').change(function () { rooms.changeRoomTotal(); });
    $('#room-features input').change(function () { rooms.filterRooms(); });
    $('#RoomType').change(function () { rooms.filterRooms(); });
    $(':radio[name="Day"]').change(function () { rooms.applyAvailabilityFilters(); });

    if (typeof selectedRooms !== "undefined") {
        var setup = true;
        $('#no_rooms').val(selectedRooms.length);
    } else {
        var setup = false;
    }
    rooms.filterRooms(setup);

    $('#SpecialRequest').prop('placeholder', "e.g. Visual Studio 2010 installed on computers ...");

    $('#StartTime').change(function () { checkTime(); });
    $('#Length').change(function () { checkTime(); });
});

function checkTime() {
    var start = parseInt($('#StartTime').val());
    var length = parseInt($('#Length').val());

    if (start + length > 18) {
        console.log((start + length));
        alert("Cannot have a request for a time this long. It exceeds the timetable available");
        $('#Length').val(18 - start);
    }
}

var rooms = {
    roomInfo: null,
    students: null,
    park: null,
    building: null,
    roomTotal: 0,
    roomType: null,

    updateInfo: function () {
        this.students = $('#StudentsTotal').val();
        this.park = $('#Park').val();
        this.building = $('#buildings').val();
        this.roomType = $('#RoomType').val()
    },

    updateBuildings: function () {
        this.updateInfo();
        //$.get('/team09web/Home/Buildings',
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
        $('.room-select').addClass('loading');
        //$.get('/team09web/Home/Rooms',
        $.get('/Home/Rooms',
            {
                students: this.students,
                park: this.park,
                building: this.building,
                facilities: this.getFacilities().join('|'),
                roomType: this.roomType
            },
            function (data) {
                rooms.roomInfo = data;
                rooms.changeSelects();
                rooms.changeRoomTotal(setup);
                rooms.applyAvailabilityFilters();
                $('.room-select').removeClass('loading');
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
        this.changeRoom();
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
        $('#room' + id).unbind('change').change(function () {
            $(this).find('select option[value="' + $(this).find('select').val() + '"]').addClass('selectable');
            rooms.changeRoom();
        });
    },

    changeRoom: function () {
        $('select option.selectable:not(:selected)').each(function () {
            $('option[value="' + $(this).attr('value') + '"').removeAttr('disabled');
            $(this).removeClass('selectable');
        });
        var select;
        var select2v;
        for (var x = 1; x <= this.roomTotal; x++) {
            select = $('#room' + x + ' select');
            for (var y = 1; y <= this.roomTotal; y++) {
                select2v = $('#room' + y + ' select').val();
                if (x == y || select2v == "" || select2v == 0 || select2v == null || select2v.indexOf("<") != -1) {
                    continue;
                }
                if (select.val() == select2v) select.prop("selectedIndex", -1);
                select.find('option[value=' + select2v + ']').prop('disabled', 'disabled');
            }
        }
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
    },

    getFacilities: function () {
        var facilities = [];
        $('#room-features input').each(function () {
            if ($(this).prop("checked")) {
                facilities.push($(this).prop("name"));
            }
        });
        return facilities;
    },

    applyAvailabilityFilters: function () {
        $('.room-select option[value!="0"]').prop("disabled", "disabled");

        var day = parseInt($(":radio[name='Day']").index($(":radio[name='Day']:checked"))) + 1;
        if (day == 0) {
            $('.room-select option').removeAttr("disabled");
            return;
        }
        var start = parseInt($('#StartTime').val());
        var slot = 'slot' + day + start;

        var park = $("#Park option:selected").text();
        if (park.indexOf('<') !== -1) park = "";
        var building = $("#Building option:selected").text();
        if (building.indexOf('<') !== -1) building = "";
        var roomType = $('#RoomType option:selected').text();
        if (roomType.indexOf('<') !== -1) roomType = "";

        //$.get('/team09web/Availability/checkslot',
        $.get('/Availability/checkslot',
            {
                parkName: park,
                buildingName: building,
                roomCode: "No",
                semester: $('#Semester').val(),
                week: this.getWeeks().join(';'),
                capacity: this.students,
                roomType: roomType,
                slot: slot
            },
            function (data) {
                data = data.split(';');
                for (var x in data) {
                    var room = data[x].trim();
                    $('.room-select option:contains("' + room + '")').removeAttr("disabled");
                }
                rooms.changeRoom();
            }
        );
    },

    getWeeks: function () {
        var weeks = [];
        for (var x = 1; x <= 15; x++) {
            if ($('#Week' + x).prop("checked")) {
                weeks.push(x);
            }
        }
        return weeks;
    }
};