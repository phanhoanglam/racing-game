import { createStore } from "vuex";

const initState = () => ({
  connectionId: null,
  userName: null,
  isHost: false,
  idRoom: null,
  usersOfRoom: [],
  text: null,
});
const store = createStore({
  state: initState,
  mutations: {
    setUserName(s, payload) {
      s.userName = payload;
    },
    setState(s, payload) {
      const user = payload.usersOfRoom.find(
        (user) => user.connectionId == payload.connectionId
      );
      s.idRoom = payload.idRoom;
      s.connectionId = payload.connectionId;
      s.userName = user.userName;
      s.isHost = user.isHost;
      s.usersOfRoom = payload.usersOfRoom;
      s.text = payload.text;
    },
    setText(s, text) {
      s.text = text;
    },
  },
  getters: {},
});

export default store;
