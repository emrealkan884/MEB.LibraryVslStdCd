<script setup lang="ts">
import { RouterView } from 'vue-router'
import Toast from 'primevue/toast'
</script>

<template>
  <div class="app-shell">
    <RouterView />
    <Toast position="top-right" />
  </div>
</template>

<style scoped>
.app-shell {
  min-height: 100vh;
  background-color: #f3f4f6;
  color: #111827;
}
</style>
