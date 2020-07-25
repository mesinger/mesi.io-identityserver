<template>
  <div></div>
</template>

<script lang="ts">
import Vue from 'vue'
import router from '../router'
import Oidc from 'oidc-client';
import Component from 'vue-class-component';

@Component
export default class LoginCallback extends Vue{
  mounted() {
    console.log("login callback");
    new Oidc.UserManager({response_mode: "query"}).signinRedirectCallback().then(() => router.push("/"));
  }
}
</script>
