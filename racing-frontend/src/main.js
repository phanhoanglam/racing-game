import { createApp } from "vue";
import Index from "./Index.vue";
import store from "./stores/game";

createApp(Index).use(store).mount("#app");
