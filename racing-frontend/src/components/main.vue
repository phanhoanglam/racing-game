<template>
  <div>
    <h1>Typing Game</h1>
    <h3 v-if="idRoom">Room id: {{ idRoom }}</h3>
    <div v-if="!userName">
      <span>Input name</span> <br />
      <input v-model="name" type="text" />
      <div>
        <button class="btn-room btn-start-game" @click="InputName">
          Complete
        </button>
      </div>
    </div>
    <div v-show="!idRoom && userName">
      <room
        :userName="userName"
        @room-created="roomCreated"
        @room-joined="roomJoined"
      ></room>
    </div>

    <div class="div-score" v-if="idRoom">
      <div v-for="(item, index) in usersOfRoom" :key="index">
        <scoreboard
          :textDecoration="
            item.connectionId === connectionId ? 'underline' : null
          "
          :userName="item.userName"
          :progressPercent="item.persent"
        ></scoreboard>
      </div>
    </div>
    <div v-if="idRoom && !text">
      <button v-if="isHost" class="btn-room btn-start-game" @click="StartGame">
        Start
      </button>
    </div>
    <game
      :text="text"
      :idRoom="idRoom"
      :connectionId="connectionId"
      @updated-percent="updatedPercent"
      v-show="text"
    ></game>
  </div>
</template>

<script>
import connection from "../signalr";
import game from "./game.vue";
import room from "./room.vue";
import scoreboard from "./scoreboard.vue";
import { mapState, mapMutations } from "vuex";

export default {
  components: { game, scoreboard, room },
  name: "gameTyping",
  data() {
    return {
      name: "",
    };
  },
  computed: mapState([
    "connectionId",
    "userName",
    "isHost",
    "idRoom",
    "usersOfRoom",
    "text",
  ]),
  methods: {
    ...mapMutations(["setUserName"]),
    InputName() {
      this.setUserName(this.name);
      this.isShowRoom = true;
    },
    StartGame() {
      connection.invoke("StartGame", this.idRoom);
    },
    updatedPercent(results) {
      this.usersOfRoom = results.usersOfRoom;
    },
  },
};
</script>

<style scoped>
.btn-room {
  border-radius: 5px;
  font-weight: bold;

  color: #fff;
  background-color: #4caf50;
  border: none;
  cursor: pointer;
  font-size: 20px;
}
.btn-room:hover {
  background-color: #3e8e41;
}

.btn-start-game {
  padding: 20px 49px;
}

input[type="text"] {
  width: 20%;
  padding: 12px 20px;
  margin: 8px 0;
  display: inline-block;
  border: 1px solid #ccc;
  border-radius: 4px;
  box-sizing: border-box;
  text-align: center;
}
</style>
