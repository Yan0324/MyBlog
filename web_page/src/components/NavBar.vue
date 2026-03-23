<template>
  <nav class="navbar">
    <div class="container nav-container">
      <ul class="nav-menu">
        <li
          v-for="(item, index) in navItems"
          :key="item.to"
          class="nav-item"
          :ref="el => { if (el) navRefs[index] = el }"
        >
          <RouterLink :to="item.to" class="nav-link" active-class="is-active">
            {{ item.label }}
          </RouterLink>
        </li>
        <li class="nav-item" :ref="el => { if (el) navRefs[navItems.length] = el }">
          <button class="theme-btn" @click="$emit('toggle-theme')">
            <i :class="['fas', theme === 'dark' ? 'fa-sun' : 'fa-moon']"></i>
          </button>
        </li>
      </ul>
    </div>
  </nav>
</template>

<script>
export default {
  name: 'NavBar',
  props: {
    theme: String
  },
  emits: ['toggle-theme'],
  data() {
    return {
      navItems: [
        { to: '/', label: 'Home' },
        { to: '/about', label: 'About' },
        { to: '/projects', label: 'Projects' },
        { to: '/contact', label: 'Contact' }
      ],
      navRefs: []
    }
  },
  mounted() {
    this.navRefs.forEach((item, index) => {
      if (item) {
        setTimeout(() => {
          item.style.transition = 'all 0.8s cubic-bezier(0.25, 0.46, 0.45, 0.94)'
          item.style.opacity = '1'
          item.style.transform = 'translateY(0)'
        }, 1000 + index * 100)
      }
    })
  }
}
</script>

<style>
/* Navigation */
.navbar {
  position: absolute;
  top: 0;
  width: 100%;
  padding: 2rem;
  z-index: 100;
}

.nav-container {
  display: flex;
  justify-content: flex-end;
}

.nav-menu {
  display: flex;
  list-style: none;
  gap: 2rem;
  align-items: center;
}

.nav-item {
  opacity: 0;
  transform: translateY(20px);
}

.nav-link {
  text-decoration: none;
  color: var(--text-secondary);
  font-family: var(--font-mono);
  font-size: 0.9rem;
  letter-spacing: 0.05em;
  transition: color 0.3s ease;
  position: relative;
  display: inline-block;
}

.nav-link:hover {
  color: var(--text-primary);
}

.nav-link.is-active {
  color: var(--text-primary);
}

.nav-link::after {
  content: '';
  position: absolute;
  bottom: -5px;
  left: 50%;
  transform: translateX(-50%) scaleX(0);
  width: 100%;
  height: 1px;
  background-color: var(--text-primary);
  transition: transform 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

.nav-link.is-active::after,
.nav-link:hover::after {
  transform: translateX(-50%) scaleX(1);
}

/* Theme Toggle */
.theme-btn {
  background: none;
  border: none;
  color: var(--text-secondary);
  cursor: pointer;
  font-size: 1rem;
  transition: color 0.3s ease;
}

.theme-btn:hover {
  color: var(--text-primary);
}

@media (max-width: 768px) {
  .navbar {
    padding: 1.5rem;
  }

  .nav-menu {
    gap: 1rem;
  }

  .nav-link {
    font-size: 0.8rem;
  }
}
</style>
