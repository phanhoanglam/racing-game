<template>
  <div class="container" @click="focusInput">
    <input
      ref="textInput"
      class="text-input-hidden"
      type="text"
      @input="handleInput"
    />
    <div class="text-box">
      <p>
        <span v-for="(char, index) in text" :key="index" :ref="'textKey'">
          {{ char }}
        </span>
      </p>
    </div>
  </div>
</template>

<script>
import connection from "../signalr";

export default {
  name: "gameVue",
  props: {
    text: {
      type: String,
      required: true,
    },
    idRoom: {
      type: String,
      required: true,
    },
    connectionId: {
      type: String,
      required: true,
    },
  },
  setup() {
    return {
      connection: null,
      currentWordIndex: 0,
    };
  },
  methods: {
    focusInput() {
      this.$nextTick(() => {
        this.$refs.textInput.focus();
      });
    },
    handleInput(event) {
      if (this.currentWordIndex < this.text.length) {
        if (event.inputType === "deleteContentBackward") {
          return;
        }
        if (this.text[this.currentWordIndex] == event.data) {
          this.$refs.textKey[this.currentWordIndex].classList.add("correct");

          if (
            this.$refs.textKey[this.currentWordIndex].classList.contains(
              "wrong"
            )
          )
            this.$refs.textKey[this.currentWordIndex].classList.remove("wrong");

          this.currentWordIndex++;
          connection.invoke(
            "UpdatePercent",
            this.currentWordIndex,
            this.connectionId,
            this.idRoom
          );
          return;
        }
        this.$refs.textKey[this.currentWordIndex].classList.add("wrong");
      }
    },
    resetData() {
      this.currentWordIndex = 0;
    },
  },
  created() {
    connection.on("UpdatedPercent", (results) => {
      this.$emit("updatedPercent", results);
    });
  },
};
</script>

<style scoped>
.text-box {
  background-color: #f6fbff;
  font-size: 25px;
  font-weight: bold;
  border: solid 2px grey;
  border-radius: 7px;
}
.text-input-hidden {
  position: absolute;
  left: -9999px;
}
.correct {
  color: green;
  text-decoration: underline;
}
.wrong {
  background-color: #f0a3a3;
}
</style>
