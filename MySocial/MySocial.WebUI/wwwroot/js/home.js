document.addEventListener("DOMContentLoaded", function () {
	document.querySelectorAll(".likeButton").forEach(button => {
		button.addEventListener("click", async function (e) {
			e.preventDefault();

			const postId = this.dataset.postId;
			const userId = this.dataset.userId;

			const svg = this.querySelector("svg");
			let isLiked = this.dataset.isLiked === "true"
			const countEl = document.getElementById(`like-count-${postId}`);

			console.log(postId, userId)
			try {
				const response = await axios.post("/Like/LikePost", {
					postId: parseInt(postId),
					userId: userId
				})
				if (response) {
					const likeState = !isLiked;
					this.dataset.isLiked = likeState.toString();

					svg.setAttribute("fill", likeState ? "red" : "#e3e3e3");

					let currentLikes = parseInt(countEl.textContent);
					countEl.textContent = likeState ? currentLikes + 1 : currentLikes - 1;
				}
			}
			catch (error) {
				console.error("Error liking post", error);
			};
		});
	});
});
