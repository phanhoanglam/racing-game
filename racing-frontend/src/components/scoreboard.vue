<template>
  <div class="container">
    <div class="text-display">
      <div class="car-name">{{ userName }}</div>
      <div :style="rankDisplay ? 'color: green' : ''">
        {{ rankDisplay ?? `${progressPercent}%` }}
      </div>
    </div>
    <div class="progress-wrapper">
      <div class="progress-container">
        <div class="progress-bar" ref="progressBar">
          <div class="car" :style="{ left: carPosition }"></div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: undefined,
  props: {
    progressPercent: {
      type: Number,
      default: 0,
    },
    userName: {
      type: String,
    },
    textDecoration: {
      type: String,
      default: "text-decoration",
    },
    rankDisplay: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      carPosition: "0px",
    };
  },
  watch: {
    progressPercent(newVal) {
      const carWidth = this.$refs.progressBar.querySelector(".car").offsetWidth;
      const maxPosition = this.$refs.progressBar.offsetWidth - carWidth;
      this.carPosition = (newVal / 100) * maxPosition + "px";
    },
  },
};
</script>

<style scoped>
.container {
  margin-bottom: 20px;
}
.text-display {
  display: flex;
  justify-content: space-between;
}
.progress-bar {
  height: 35px;
  position: relative;
  overflow: hidden;
  border-bottom: dashed red;
}

.car {
  width: 60px;
  height: 25px;
  background-image: url(https://data.typeracer.com/public/images/avatars/basic-pink.svg);
  display: inline-block;
  background-size: contain;
  background-repeat: no-repeat;
  background-position: center center;
  position: absolute;
  top: 5px;
  transition: left 0.3s;
}

.car-name {
  font-weight: bold;
}
</style>
