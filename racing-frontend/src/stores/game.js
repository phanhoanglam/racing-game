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
        (user) => user.connectionId === payload.connectionId
      );
      s.idRoom = payload.idRoom;
      if (!s.connectionId) {
        s.connectionId = payload.connectionId;
        s.userName = user.userName;
        s.isHost = user.isHost;
      }
      s.usersOfRoom = payload.usersOfRoom;
      s.text = payload.text;
    },
    setText(s, text) {
      s.text = text;
    },
    setRoom(s, payload) {
      s.usersOfRoom = s.usersOfRoom.filter(function (obj) {
        return obj.connectionId !== payload;
      });
    },
    reset(s) {
      s.usersOfRoom.forEach((u) => {
        u.persent = 0;
        u.rank = 0;
        u.rankDisplay = null;
        u.wpm = 0;
      });
      s.text = null;
    },
  },
  getters: {},
});

export default store;
