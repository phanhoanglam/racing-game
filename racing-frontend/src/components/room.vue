<template>
  <label>Room id:</label> <br />
  <input v-model="roomId" type="text" />
  <div>
    <button :disabled="!roomId" class="btn-enter-room" @click="EnterRoom">
      Enter the room
    </button>
  </div>
  <div>
    <button class="btn-create-room" @click="CreateRoom">Create room</button>
  </div>
</template>

<script>
import connection from "../signalr";

export default {
  name: "roomVue",
  props: {
    userName: {
      type: String,
    },
  },
  data() {
    return {
      roomId: "",
    };
  },
  methods: {
    CreateRoom() {
      connection.invoke("CreateRoom", this.userName);
    },
    EnterRoom() {
      connection.invoke("JoinRoom", this.roomId, this.userName);
    },
  },
};
</script>

<style scoped>
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
.btn-create-room {
  padding: 20px 60px;
}
.btn-enter-room {
  padding: 20px 49px;
  margin: 20px 0px;
}
button:disabled,
button[disabled] {
  border: 1px solid #999999;
  background-color: #cccccc;
  color: #666666;
}
</style>
