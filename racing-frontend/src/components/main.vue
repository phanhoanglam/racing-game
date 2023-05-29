<template>
  <div>
    <h1>Typing Game</h1>
    <h3 v-if="idRoom">Room id: {{ idRoom }}</h3>
    <div v-if="!userName">
      <span>Input name</span> <br />
      <input v-model="name" type="text" />
      <div>
        <button
          :disabled="!name"
          class="btn-room btn-start-game"
          @click="InputName"
        >
          In game
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
          :rankDisplay="item.rank == 0 ? null : item.rankDisplay"
        ></scoreboard>
      </div>
    </div>
    <div v-if="idRoom && !text">
      <button v-if="isHost" class="btn-room btn-start-game" @click="StartGame">
        Start
      </button>
    </div>
    <game
      ref="gameComponent"
      :text="text"
      :idRoom="idRoom"
      :connectionId="connectionId"
      @updated-percent="UpdatedPercent"
      v-show="text"
    ></game>
    <button
      class="btn-room btn-start-game"
      style="margin-top: 20px"
      @click="RaceAgain"
      v-if="isShowRaceAgain"
    >
      Race again
    </button>
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
    const name = sessionStorage.getItem("userName");
    if (name) this.setUserName(name);
    return {
      name,
      currentWordIndex: 0,
    };
  },
  computed: {
    ...mapState([
      "connectionId",
      "userName",
      "isHost",
      "idRoom",
      "usersOfRoom",
      "text",
    ]),
    isShowRaceAgain() {
      return (
        this.isHost &&
        this.usersOfRoom.length &&
        this.usersOfRoom.every((u) => u.rank !== 0)
      );
    },
  },
  methods: {
    ...mapMutations(["setUserName", "reset"]),
    InputName() {
      sessionStorage.setItem("userName", this.name);
      this.setUserName(this.name);
    },
    StartGame() {
      connection.invoke("StartGame", this.idRoom);
    },
    UpdatedPercent(results) {
      this.usersOfRoom = results.usersOfRoom;
    },
    RaceAgain() {
      this.$refs.gameComponent.resetData();
      this.reset();
    },
  },
  created() {
    connection.on("ReceiveError", (result) => {
      alert(result);
      this.roomId = "";
    });
  },
};
</script>

<style scoped>
button {
  border-radius: 5px;
  font-weight: bold;

  color: #fff;
  background-color: #4caf50;
  border: none;
  cursor: pointer;
  font-size: 20px;
}
button:hover {
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

button:disabled,
button[disabled] {
  border: 1px solid #999999;
  background-color: #cccccc;
  color: #666666;
}
</style>
