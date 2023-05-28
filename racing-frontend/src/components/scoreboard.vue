<template>
  <div>
    <div class="progress-container">
      <span
        class="progress-text"
        :style="'text-decoration: ' + textDecoration"
        >{{ userName }}</span
      >
      <progress
        class="progress-percent"
        max="100"
        :value="progressPercent"
      ></progress>
      <span class="progress-text">{{ progressPercent }}%</span>
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
  },
  data() {
    return {};
  },
};
</script>

<style scoped>
.progress-percent {
  padding: 20px 0px;
  flex: 1;
}
.progress-container {
  display: flex;
  align-items: center;
}

.progress-text {
  font-weight: bold;
  width: 10%;
}
</style>
