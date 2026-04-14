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
      navRefs: [],
      revealTimers: []
    }
  },
  mounted() {
    if (window.matchMedia('(prefers-reduced-motion: reduce)').matches) {
      this.navRefs.forEach((item) => {
        if (!item) return
        item.style.opacity = '1'
        item.style.transform = 'translateY(0)'
      })
      return
    }

    this.navRefs.forEach((item, index) => {
      if (!item) return

      const timer = window.setTimeout(() => {
        item.style.transition = 'opacity 0.7s cubic-bezier(0.22, 1, 0.36, 1), transform 0.7s cubic-bezier(0.22, 1, 0.36, 1)'
        item.style.opacity = '1'
        item.style.transform = 'translateY(0)'
      }, 320 + index * 90)

      this.revealTimers.push(timer)
    })
  },
  beforeUnmount() {
    this.revealTimers.forEach((timer) => clearTimeout(timer))
  }
}
</script>

<style>
/* Navigation */
.navbar {
  position: relative;
  width: 100%;
  height: 100%;
  padding: 1rem 1.5rem;
  z-index: 1;
}

.nav-container {
  height: 100%;
  display: flex;
  align-items: center;
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
  transform: translateY(-16px);
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
  transition: color 0.3s ease, transform 0.3s ease;
}

.theme-btn:hover {
  color: var(--text-primary);
  transform: translateY(-2px);
}

@media (max-width: 768px) {
  .navbar {
    padding: 0.85rem 1rem;
  }

  .nav-menu {
    gap: 0.85rem;
  }

  .nav-link {
    font-size: 0.8rem;
  }
}
</style>
