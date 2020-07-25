<template>
  <div>
    <OpenIdConnect />
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from 'vue-class-component';
import OpenIdConnect from '@/components/OpenIdConnet.vue';

@Component({
  components: {
    OpenIdConnect
  }
})
export default class Home extends Vue{
  
}
</script>