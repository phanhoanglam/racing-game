<template>
  <input v-model="roomId" type="text" />
  <div>
    <button class="btn-room btn-enter-room" @click="EnterRoom">
      Enter the room
    </button>
  </div>
  <div>
    <button class="btn-room btn-create-room" @click="CreateRoom">
      Create room
    </button>
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
.btn-create-room {
  padding: 20px 60px;
}
.btn-enter-room {
  padding: 20px 49px;
  margin: 20px 0px;
}
</style>
