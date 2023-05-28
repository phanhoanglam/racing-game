import * as signalR from "@microsoft/signalr";
import store from "./stores/game";

const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:7078/chatHub")
  .build();

connection
  .start()
  .then(() => {
    console.log("Kết nối thành công với SignalR.");
  })
  .catch((error) => {
    console.error("Không thể kết nối với SignalR:", error);
  });

connection.on("ReceiveDataGame", (results) => {
  store.commit("setState", {
    idRoom: results.idRoom,
    connectionId: results.connectionId,
    usersOfRoom: results.usersOfRoom,
    text: results.text,
  });
});
export default connection;
